import argparse
import os
import requests
import time
from bs4 import BeautifulSoup
import json

parser = argparse.ArgumentParser(description="crawaling")

parser.add_argument('--type', type=str, default="", help='crawling type')
args = parser.parse_args()

base_url = "https://sideload.betterrepack.com/download/AISHS2/Sideloader%20Modpack/"
json_map_path = "etc_knowledges.json"

if args.type == "map":
    base_url = "https://sideload.betterrepack.com/download/AISHS2/Sideloader%20Modpack%20-%20Maps/"
    json_map_path = "map_knowledges.json"

knowledges = []

def get_page(url, wait_time):
    # request url
    time.sleep(wait_time)
    response = requests.get(url + "/" + "?C=M;O=D")
    soup = BeautifulSoup(response.text, 'html.parser')

    table = soup.find('table', id='indexlist')

    results = []

    if table:
        rows = table.find_all('tr', class_=['even', 'odd'])
        for row in rows:
            name = ""
            href = ""
            date = ""
            size = ""
            td = row.find('td', class_='indexcolname')
            if td and td.a:
                name = td.a.text.strip()
                href = td.a.get('href') 
                
            td = row.find('td', class_='indexcollastmod') 
            if td:
                date = td.text.strip()
            
            td = row.find('td', class_='indexcolsize') 
            if td:            
                size = td.text.strip()            
            
            results.append((name, href, date, size))             
    else:
        print("table#indexlist not found")
        
    return results

if os.path.exists(json_map_path):
    with open(json_map_path, "r", encoding="utf-8") as f:
        knowledges = json.load(f)
        
existing_urls = {item['author_url'] for item in knowledges}
        
# get result
page_urls = get_page(base_url, 1.0)

print(f"total site {len(page_urls)}")
print(f"remain site {len(page_urls) - len(existing_urls)}")

for url, href,_,_ in page_urls:
    if "/" in url:
        if base_url + url not in existing_urls:
            list = get_page(base_url + url, 5)
            
            for item, href, date, size in list:
                if "Parent Directory" not in item:
                    knowledge = {
                        "author": url.rstrip("/"),
                        "title": item,
                        "author_url": base_url + url,
                        "mod_url": base_url + url + href,
                        "date": date,
                        "size": size
                        
                    }
                    
                    knowledges.append(knowledge)
                    with open(json_map_path, "w", encoding="utf-8") as f:
                        json.dump(knowledges, f, ensure_ascii=False, indent=4)                
                    
            print(f"done " + url.rstrip("/"))
                            
                                    
print(f"created {json_map_path}")