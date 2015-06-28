__author__ = 'ankit'

import twitterConfig
import yweather

twitter_api = twitterConfig.TwitterLogin().login_to_twitter()


def get_woeid(location):

    client = yweather.Client()
    return client.fetch_woeid(location)


def get_tweets(woeid_str):

    woeid = int(woeid_str.strip())
    trends = twitter_api.trends.place(_id=woeid)
    trends_set = set([trend['name']
                    for trend in trends[0]['trends']])
    tweets_for_each_trending_topic = dict()
    for trend in trends_set:
        tweets = twitter_api.search.tweets(q=trend, count=1)
        temp = []
        for tweet in tweets:
            temp.append(tweet)
        tweets_for_each_trending_topic[trend] = tweets['statuses'][0]['text'].encode("utf8")
    print tweets_for_each_trending_topic




if __name__ == '__main__':
    woeid = get_woeid('India')
    # print woeid
    get_tweets(woeid)
    # print type(US_WOE_ID)



