import os
from flask import Flask, request, jsonify, render_template, redirect, url_for, session, request
from werkzeug.security import generate_password_hash, check_password_hash
from dotenv import load_dotenv
from functools import wraps
from datetime import timedelta, datetime
from flask_jwt_extended import (
    JWTManager,
    create_access_token,
    create_refresh_token,
    jwt_required,
    get_jwt_identity,
    get_jwt,
    decode_token
)

import json


LOGS_PER_PAGE = 25

cached_all_logs = []
log_all_offset = 0

cached_attack_logs = []
log_attack_offset = 0

load_dotenv()

from models import db, Users, Progress, func

app = Flask(__name__)

DATABASE_URI = f"mysql+pymysql://{os.environ.get('MYSQL_USER')}:{os.environ.get('MYSQL_PASSWORD')}@" \
               f"{os.environ.get('MYSQL_CONTAINER_NAME')}:{os.environ.get('MYSQL_PORT')}/" \
               f"{os.environ.get('MYSQL_DATABASE')}"
               
app.config['SQLALCHEMY_DATABASE_URI'] = DATABASE_URI
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SECRET_KEY'] = os.urandom(32).hex()
app.config["JWT_ACCESS_TOKEN_EXPIRES"] = timedelta(hours=24)
app.config["JWT_SECRET_KEY"] = os.environ.get("JWT_SECRET_KEY", "secret_jwt")

jwt = JWTManager(app)
db.init_app(app)


def role_required(role):
    def decorator(fn):
        @wraps(fn)
        @jwt_required() 
        def wrapper(*args, **kwargs):
            claims = get_jwt()
            if claims.get("role", None) != role:
                return jsonify({'error': "You don't have permission to access this resource!"}), 403
            return fn(*args, **kwargs)
        return wrapper
    return decorator

def web_session_required(role=None):
    def decorator(fn):
        @wraps(fn)
        def wrapper(*args, **kwargs):
            token = session.get('access_token')
            if not token:
                return redirect(url_for('admin_login'))
            try:
                decoded_token = decode_token(token)
            except Exception as e:
                app.logger.error(f"Token decoding error: {e}")
                return redirect(url_for('admin_login'))
            if role and decoded_token.get('role') != role:
                return jsonify({'error': "You don't have permission to access this resource!"}), 403
            return fn(*args, **kwargs)
        return wrapper
    return decorator

@app.route('/register', methods=['POST'])
def register():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')
    institution = data.get('institution_name')

    if not all([username, password, institution]):
        return jsonify({'error': 'Fields missing!'}), 400

    if Users.query.filter_by(username=username).first():
        return jsonify({'error': 'Username already exists!'}), 400

    hashed_password = generate_password_hash(password)

    new_user = Users(username=username,  password=hashed_password, 
                     institution_name=institution, role='user')
    db.session.add(new_user)
    db.session.commit()

    new_progress = Progress(user_id=new_user.id, progress=1, timestamp=datetime.utcnow())
    db.session.add(new_progress)
    db.session.commit()

    return jsonify({'message': 'User registered successfully!'}), 201

@app.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')

    if not all([username, password]):
        return jsonify({'error': 'Username or password missing!'}), 400

    user = Users.query.filter_by(username=username).first()
    if not user or not check_password_hash(user.password, password):
        return jsonify({'error': 'Username or password incorrect!'}), 401

    additional_claims = {"role": user.role}
    access_token = create_access_token(identity=str(user.id), expires_delta=timedelta(hours=24),
                                       additional_claims=additional_claims)
    refresh_token = create_refresh_token(identity=str(user.id))

    return jsonify({
        'message': 'Login successful!',
        'access_token': access_token,
        'refresh_token': refresh_token
    }), 200

@app.route('/refresh', methods=['POST'])
@jwt_required(refresh=True)
def refresh():
    current_user = get_jwt_identity()
    new_access_token = create_access_token(identity=current_user)
    return jsonify({'access_token': new_access_token}), 200

@app.route('/user/progress', methods=['GET', 'POST'])
@jwt_required()
def get_last_progress():
    if request.method == 'GET':
        try:
            user_id = int(get_jwt_identity())
        except (TypeError, ValueError):
            return jsonify({"error": "Invalid user identity"}), 400

        last_progress = db.session.query(func.max(Progress.progress)).filter(Progress.user_id == user_id).scalar()
        
        if last_progress is None:
            return jsonify({"error": "No progress found for user"}), 404
        
        return jsonify({"user_id": user_id, "progress_level": last_progress}), 200
    if request.method == 'POST':
        user_id = get_jwt_identity()
        data = request.get_json()
        progress_val = data.get('progress')
        if progress_val is None:
            return jsonify({'error': 'Progress value missing'}), 400

        try:
            progress_val = int(progress_val)
        except ValueError:
            return jsonify({'error': 'Progress value must be an integer'}), 400

        new_progress = Progress(user_id=user_id, progress=progress_val, timestamp=datetime.utcnow())
        db.session.add(new_progress)
        db.session.commit()

        return jsonify({'message': 'Progress registered successfully!'}), 201

@app.route('/admin/user_stats/<int:user_id>')
@web_session_required() 
def user_stats(user_id):
    records = Progress.query.filter_by(user_id=user_id).order_by(Progress.timestamp.desc()).all()
    stats = []
    for record in records:
        stats.append({
            'progress': record.progress,
            'timestamp': record.timestamp.strftime("%Y-%m-%d %H:%M:%S")
        })
    return jsonify(stats)

@app.route('/admin/login', methods=['GET', 'POST'])
def admin_login():
    if request.method == 'POST':
        username = request.form.get('username')
        password = request.form.get('password')

        if not all([username, password]):
            return jsonify({'error': 'Username or password missing!'}), 400

        user = Users.query.filter_by(username=username).first()
        if not user or not check_password_hash(user.password, password) or user.role != 'admin':
            return jsonify({'error': 'Username or password incorrect!'}), 401

        additional_claims = {"role": user.role}
        access_token = create_access_token(
            identity=str(user.id),
            expires_delta=timedelta(hours=24),
            additional_claims=additional_claims
        )
        refresh_token = create_refresh_token(identity=str(user.id))

        session['access_token'] = access_token
        session['refresh_token'] = refresh_token

        return redirect(url_for('admin_users'))

    return render_template('login.html')

@app.route('/admin/dashboard')
@web_session_required('admin')
def admin_users():
    app.logger.debug('Mesaj de debug: Ruta "/admin/dashboard" a fost accesată.')
    search_query = request.args.get('q', '')
    page = request.args.get('page', 1, type=int)
    
    if search_query:
        query = Users.query.filter(
            (Users.username.ilike(f"%{search_query}%")) |
            (Users.institution_name.ilike(f"%{search_query}%"))
        )
    else:
        query = Users.query

    users_paginate = query.paginate(page=page, per_page=15, error_out=False)
    total_users = users_paginate.total

    return render_template(
        'admin.html',
        users=users_paginate.items,
        total_users=total_users,
        search_query=search_query,
        pagination=users_paginate
    )

@app.route('/admin/view_progress')
@web_session_required('admin')
def admin_view_progress():
    search_query = request.args.get('q', '')
    page = request.args.get('page', 1, type=int)

    query = db.session.query(
        Users.id,
        Users.username,
        Users.institution_name,
        func.max(Progress.progress).label('max_progress'),
        func.max(Progress.timestamp).label('max_progress_timestamp')
    ).join(Progress).group_by(Users.id)

    if search_query:
        query = query.filter(
            (Users.institution_name.ilike(f"%{search_query}%")) |
            (Users.username.ilike(f"%{search_query}%"))
        )
    
    users_paginate = query.paginate(page=page, per_page=15, error_out=False)
    
    return render_template('admin_progress.html',
                           users=users_paginate.items,
                           total_users=users_paginate.total,
                           search_query=search_query,
                           pagination=users_paginate)


def parse_log_time(log_entry):
    try:
        time_str = log_entry.get('transaction', {}).get('time')
        return datetime.strptime(time_str, "%d/%b/%Y:%H:%M:%S.%f %z")
    except Exception:
        return datetime.min

def update_logs():
    global log_all_offset, cached_all_logs
    log_file = '/app/logs/modsec_audit.log'
    try:
        with open(log_file, 'r') as f:
            f.seek(log_all_offset)
            new_lines = f.readlines()
            log_all_offset = f.tell()
            for line in new_lines:
                try:
                    log_entry = json.loads(line)
                    cached_all_logs.append(log_entry)
                except json.JSONDecodeError:
                    continue
    except Exception as e:
        print("Reading log file error:", e)

@app.route('/admin/logs/all')
@web_session_required('admin')
def admin_all_logs():
    update_logs()

    ip_filter      = request.args.get('ip',      '').strip()
    method_filter  = request.args.get('method',  '').strip().upper()

    logs = sorted(cached_all_logs, key=parse_log_time, reverse=True)

    if ip_filter:
        logs = [
            l for l in logs
            if ip_filter in l.get('transaction', {}).get('remote_address', '')
        ]

    if method_filter:
        logs = [
            l for l in logs
            if l.get('request', {}).get('request_line', '').startswith(method_filter)
        ]

    page        = request.args.get('page', 1, type=int)
    total_logs  = len(logs)
    total_pages = (total_logs + LOGS_PER_PAGE - 1) // LOGS_PER_PAGE
    start, end  = (page - 1) * LOGS_PER_PAGE, (page * LOGS_PER_PAGE)
    paginated   = logs[start:end]

    return render_template(
        'admin_logs_all.html',
        logs=paginated,
        page=page,
        total_pages=total_pages,
        ip_filter=ip_filter,
        method_filter=method_filter
    )


# @app.route('/admin/logs/all')
# def admin_all_logs():
#     update_logs()
#     sorted_logs = sorted(cached_all_logs, key=lambda log: parse_log_time(log), reverse=True)

#     page = request.args.get('page', 1, type=int)
#     total_logs = len(sorted_logs)
#     total_pages = (total_logs + LOGS_PER_PAGE - 1) // LOGS_PER_PAGE
#     start = (page - 1) * LOGS_PER_PAGE
#     end = start + LOGS_PER_PAGE
#     paginated_logs = sorted_logs[start:end]

#     return render_template('admin_logs_all.html', logs=paginated_logs, page=page, total_pages=total_pages)



def update_logs_attack():
    global log_attack_offset, cached_attack_logs
    log_file = '/app/logs/modsec_audit.log'
    try:
        with open(log_file, 'r') as f:
            f.seek(log_attack_offset)
            new_lines = f.readlines()
            log_attack_offset = f.tell()
            for line in new_lines:
                try:
                    log_entry = json.loads(line)
                    cached_attack_logs.append(log_entry)
                except json.JSONDecodeError:
                    continue
    except Exception as e:
        print("Reading log error:", e)

# @app.route('/admin/logs/attacks')
# def admin_logs_attacks():
#     update_logs_attack()

#     filtered_logs = []
#     for log in cached_attack_logs:
#         messages = log.get('audit_data', {}).get('messages')
#         if (messages and len(messages) > 0):
#             filtered_logs.append(log)

#     sorted_logs = sorted(filtered_logs, key=lambda log: parse_log_time(log), reverse=True)

#     page = request.args.get('page', 1, type=int)
#     total_logs = len(sorted_logs)
#     total_pages = (total_logs + LOGS_PER_PAGE - 1) // LOGS_PER_PAGE
#     start = (page - 1) * LOGS_PER_PAGE
#     end = start + LOGS_PER_PAGE
#     paginated_logs = sorted_logs[start:end]

#     return render_template('admin_logs_attacks.html', logs=paginated_logs, page=page, total_pages=total_pages)

@app.route('/admin/logs/attacks')
@web_session_required('admin')
def admin_logs_attacks():
    update_logs_attack()

    ip_filter     = request.args.get('ip', '').strip()
    method_filter = request.args.get('method', '').strip().upper()

    logs = [
        l for l in cached_attack_logs
        if l.get('audit_data', {}).get('messages')
    ]

    if ip_filter:
        logs = [
            l for l in logs
            if ip_filter in l.get('transaction', {}).get('remote_address', '')
        ]

    if method_filter:
        logs = [
            l for l in logs
            if l.get('request', {}).get('request_line', '').startswith(method_filter)
        ]

    logs        = sorted(logs, key=parse_log_time, reverse=True)
    page        = request.args.get('page', 1, type=int)
    total_logs  = len(logs)
    total_pages = (total_logs + LOGS_PER_PAGE - 1) // LOGS_PER_PAGE
    start, end  = (page - 1) * LOGS_PER_PAGE, page * LOGS_PER_PAGE
    paginated   = logs[start:end]

    return render_template(
        'admin_logs_attacks.html',
        logs=paginated,
        page=page,
        total_pages=total_pages,
        ip_filter=ip_filter,
        method_filter=method_filter
    )


@app.route('/admin/logout')
@web_session_required('admin')
def admin_logout():
    session.clear()
    return redirect(url_for('admin_login'))

if __name__ == '__main__':
    with app.app_context():
        db.create_all()
    app.run(
        host=os.environ.get("WEBSERVER_HOST"),
        port=os.environ.get("WEBSERVER_PORT"),
        debug=os.environ.get('WEBSERVER_DEBUG')
    )
