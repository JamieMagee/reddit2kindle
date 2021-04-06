from flask_wtf import FlaskForm
from flask_wtf.csrf import CSRFProtect
from wtforms import StringField, IntegerField, SelectField, BooleanField

csrf = CSRFProtect()


class Submission(FlaskForm):
    submission = StringField('Submission URL')
    comments = BooleanField('Include comments')
    comments_style = SelectField('Comments style', choices=[('numbers', 'numbers'), ('quotes', 'quotes')])
    email = StringField('Kindle email address')
    kindle_address = SelectField('Kindle address', choices=[('normal', '@kindle.com'), ('free', '@free.kindle.com')])


class Subreddit(FlaskForm):
    subreddit = StringField('Subreddit')
    comments = BooleanField('Include comments')
    comments_style = SelectField('Comments style', choices=[('numbers', 'numbers'), ('quotes', 'quotes')])
    time = SelectField('Time period',
                       choices=[('all', 'all'), ('year', 'year'), ('month', 'month'), ('week', 'week'), ('day', 'day'),
                                ('hour', 'hour')], default='week')
    limit = IntegerField('Number of posts')
    email = StringField('Kindle email address')
    kindle_address = SelectField('Kindle address', choices=[('normal', '@kindle.com'), ('free', '@free.kindle.com')])
