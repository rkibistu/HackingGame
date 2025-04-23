from flask_sqlalchemy import SQLAlchemy
from sqlalchemy import func

db = SQLAlchemy()

class Users(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(255), unique=True, nullable=False)
    password = db.Column(db.String(1024), nullable=False)  
    institution_name = db.Column(db.String(1024), nullable=True)
    role = db.Column(db.String(255), nullable=False)

class Progress(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)
    user = db.relationship('Users', backref=db.backref('progress', lazy=True))
    progress = db.Column(db.Integer, nullable=False)
    timestamp = db.Column(db.DateTime, nullable=False)