reddit2kindle 
=============
[![Libraries.io for GitHub](https://img.shields.io/librariesio/github/JamieMagee/reddit2kindle.svg)](https://libraries.io/github/JamieMagee/reddit2kindle)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://opensource.org/licenses/MIT)

A web app to convert a reddit post and comments into an e-book, and send it directly to your kindle device or app.

Now you can read your favourite subreddits from the comfort of your e-reader. Here is a preview of the web app and the output it produces.

![Web app](https://i.imgur.com/NQfG9Nt.png)

![Output](http://i.imgur.com/u5Vpkpq.png)

Installation
============
1. Install [Python 3](https://www.python.org/downloads)
2. Install the required packages `pip install -r requirements.txt`
3. Either create a `settings.cfg` file of the format:

        [smtp_auth]
        username=gmailusername
        password=gmailpassword
        [smtp]
        server=smtp.gmail.com
        port=587
        [mercury]
        token=qwertyuiopasdfghjklzxcvbnm
        [reddit]
        client_id=createyourown(see Reddit app registration section below)
        client_secret=createyourown(see Reddit app registration section below)
or create environment variables for `SMTP_USERNAME`,`SMTP_PASSWORD`, `SMTP_SERVER`, `SMTP_PORT`, `MERCURY_TOKEN`, `CLIENT_ID`, and `CLIENT_SECRET`.

4. Run the app `python reddit2kindle.py`

Usage
=====
You need to add reddit2kindle to your approved Kindle e-mail senders. To do this:

1. Visit [Manage Your Content and Devices](http://www.amazon.co.uk/manageyourkindle) page.
2. Go to "Your Account", select "Manage Your Content and Devices" and then select "Personal Document Settings".
3. Under "Approved Personal Document E-mail List" click on "Add a new approved e-mail address".
4. Add "convert@reddit2kindle.com" and select "Add Address".


Reddit APP Registration
======

You need to create a reddit API app using your reddit account for this script to work. To create one: 

Go to: https://github.com/reddit-archive/reddit/wiki/OAuth2
Copy and Paste the client secret and app id in settings.cfg