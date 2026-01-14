import sys
import json
from urllib.parse import urlparse, quote
import urllib.request

if len(sys.argv) < 3:
    print("Usage: python main.py <domain-or-url> <api-key> [--explain]")
    sys.exit(1)

input_value = sys.argv[1]
api_key = sys.argv[2]
explain = "--explain" in sys.argv[3:]

domain = input_value
try:
    parsed = urlparse(input_value)
    if parsed.hostname:
        domain = parsed.hostname
except Exception:
    pass

base_url = "https://abusesignalsapi.analyses-web.com"
url = f"{base_url}/abuse-signals?domain={quote(domain)}" + ("&explain=1" if explain else "")

req = urllib.request.Request(url, headers={"X-API-Key": api_key})

try:
    with urllib.request.urlopen(req, timeout=20) as resp:
        body = resp.read().decode("utf-8", errors="replace")
        print(f"HTTP {resp.status}\n")
except urllib.error.HTTPError as e:
    body = e.read().decode("utf-8", errors="replace")
    print(f"HTTP {e.code}\n")

try:
    print(json.dumps(json.loads(body), indent=2))
except Exception:
    print(body)
