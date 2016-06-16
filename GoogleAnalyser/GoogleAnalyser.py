import datetime
import requests
import bs4

google_url = "http://google.co.uk/search?q="

def get_search_results(query):
    ret = []
    r = requests.get(google_url + sanitise(query))
    soup = bs4.BeautifulSoup(r.text, "html.parser")
    si = extract_search_instance(soup, query)
    for result in soup.select("div.g"):
        processed = (process_result(result, si))
        if processed is not None:
            ret.append(processed)
    return ret

def sanitise(query):
    return ''.join([c if c not in ['_', ' '] else '+' for c in query])

def extract_search_instance(soup, query):
    count = 0
    for word in soup.select('#resultStats')[0].text.split():
        try:
            count = int(''.join([c for c in word if c != ',']))
            break
        except Exception as e:
            # not there
            continue
    return SearchInstance(datetime.date.today(), query, count)

def process_result(result, si):
    try:
        title = result.select_one("h3.r").text
        url   = result.select_one("cite").text
        desc  = result.select_one("span.st").text
    except:
        # this is a sidebar not a result
        return None
    return Result(url, title, desc, 1, si)

class SearchInstance:
    def __init__(self, retr, query, count):
        self.retrieved = retr
        self.query = query
        self.count = count

class Result:
    def __init__(self, url, title, desc, pageno, si):
        self.url = url
        self.title = title
        self.desc = desc
        self.pageno = pageno
        self.si = si

results = get_search_results("Flying Colors")
print(len(results))
input("Press enter to exit")