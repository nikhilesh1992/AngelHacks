__author__ = 'ankit'

import twitterConfig

twitter_api = twitterConfig.TwitterLogin().login_to_twitter()
# US_WOE_ID = 23424977
# us_trends = twitter_api.trends.place(_id=US_WOE_ID)


def get_tweets(location):

    response = twitter_api.GetSearch(term=location, count=100)
    print response


if __name__ == '__main__':
    get_tweets('Toronto')



