#!/bin/bash

cp -p /etc/modsecurity/modsecurity.conf-recommended /etc/modsecurity/modsecurity.conf

sed -i 's/SecRuleEngine DetectionOnly/SecRuleEngine On/' /etc/modsecurity/modsecurity.conf

sed -i 's/SecAuditEngine RelevantOnly/SecAuditEngine On/' /etc/modsecurity/modsecurity.conf
echo "SecAuditLogParts ABCDEFHIJZ" >> /etc/modsecurity/modsecurity.conf
echo "SecAuditLogFormat JSON" >> /etc/modsecurity/modsecurity.conf

git clone https://github.com/coreruleset/coreruleset.git
cp coreruleset/rules/* /etc/modsecurity/crs/
