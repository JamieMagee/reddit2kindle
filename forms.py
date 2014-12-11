from flask_wtf import Form
from wtforms import StringField, IntegerField, SelectField, BooleanField
from flask_wtf.csrf import CsrfProtect

csrf = CsrfProtect()


class Submission(Form):
    submission = StringField('Submission URL')
    comments = BooleanField('Include comments')
    email = StringField('Kindle email address')
    kindle_address = SelectField('Kindle address', choices=[('normal', '@kindle.com'), ('free', '@free.kindle.com')])


class Subreddit(Form):
    subreddit = StringField('Subreddit')
    time = SelectField('Time period',
                       choices=[('all', 'all'), ('year', 'year'), ('month', 'month'), ('week', 'week'), ('day', 'day'),
                                ('hour', 'hour')])
    limit = IntegerField('Number of posts')
    email = StringField('Kindle email address')
    kindle_address = SelectField('Kindle address', choices=[('normal', '@kindle.com'), ('free', '@free.kindle.com')])