__author__ = 'ankit'

import twitter



class TwitterLogin:

    def __init__(self):
        return

    def login_to_twitter(self):

        auth = twitter.oauth.OAuth(OAUTH_TOKEN, OAUTH_TOKEN_SECRET,
                           CONSUMER_KEY, CONSUMER_SECRET)
        twitter_api = twitter.Twitter(auth=auth)

        return twitter_api

