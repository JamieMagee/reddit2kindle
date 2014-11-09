from flask_wtf import Form
from wtforms import StringField
from flask_wtf.csrf import CsrfProtect

csrf = CsrfProtect()


class Submission(Form):
    submission = StringField('Submission URL')
    email = StringField('Kindle email address')