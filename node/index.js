const input = process.argv[2];
const apiKey = process.argv[3];
const explain = process.argv.includes("--explain");

if (!input || !apiKey) {
  console.log("Usage: node index.js <domain-or-url> <api-key> [--explain]");
  process.exit(1);
}

let domain = input;
try {
  const u = new URL(input);
  if (u.hostname) domain = u.hostname;
} catch {
  // not a URL, keep as-is
}

const baseUrl = "https://abusesignalsapi.analyses-web.com";
const url = `${baseUrl}/abuse-signals?domain=${encodeURIComponent(domain)}${explain ? "&explain=1" : ""}`;

(async () => {
  const res = await fetch(url, {
    headers: {
      "X-API-Key": apiKey
    }
  });

  const text = await res.text();
  console.log(`HTTP ${res.status} ${res.statusText}\n`);

  try {
    console.log(JSON.stringify(JSON.parse(text), null, 2));
  } catch {
    console.log(text);
  }
})();
