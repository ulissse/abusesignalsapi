# Abuse Signals API

Deterministic, signals-only abuse indicators for **domains** or **emails**.  
Two endpoints. Fixed OpenAPI contract. Optional **explain mode**.

Docs / Pricing (official):
- https://abusesignalsapi.analyses-web.com/docs.html
- https://abusesignalsapi.analyses-web.com/pricing.html
- OpenAPI (fixed contract): https://abusesignalsapi.analyses-web.com/openapi.yaml

---

## Quick start (curl)

curl "https://abusesignalsapi.analyses-web.com/abuse-signals?domain=fake-buy-info-product-example.shop" \
  -H "X-API-Key: YOUR_API_KEY"

Response (example):

{
  "input": { "email": null, "domain": "fake-buy-info-product-example.shop" },
  "abuseScore": 75,
  "abuseLevel": "high",
  "signals": [
    "disposable_domain_pattern",
    "low_domain_age",
    "many_hyphens_or_digits",
    "lookup_failed_rdap"
  ],
  "version": "v1",
  "meta": {
    "cached": true,
    "processingMs": 0,
    "requestId": "0HNIGUL0CKSAC:00000001",
    "cacheHours": 24,
    "scoringPolicy": "2026-01"
  }
}

---

## Explain mode

curl "https://abusesignalsapi.analyses-web.com/abuse-signals?domain=fake-buy-info-product-example.shop&explain=1" \
  -H "X-API-Key: YOUR_API_KEY"

Response includes an `explanations` array with deterministic severity + category (risk/warning) and a short note.

---

## Endpoints

- GET /abuse-signals
  - Query: exactly one of `email` or `domain`
  - Optional: `explain=1` or `explain=true`
- GET /usage
  - Returns monthly usage for the authenticated API key

---

## Suggested integration (example policy)

- abuseScore >= 80: block / manual review
- 50–79: require captcha or additional verification
- < 50: allow

(Example only. This API provides signals, not advice.)

---

## Signals (v1)

- disposable_domain_pattern
- low_domain_age
- shared_asn_with_abuse
- punycode_domain
- many_hyphens_or_digits
- lookup_failed_dns (no score impact)
- lookup_failed_asn (no score impact)
- lookup_failed_rdap (no score impact)

Signals are indicators, not verdicts.

---

## Usage

curl "https://abusesignalsapi.analyses-web.com/usage" \
  -H "X-API-Key: YOUR_API_KEY"

---

## Notes

- Deterministic outputs (signals-only). No guarantees.
- Rate limiting and monthly quotas apply per API key.
- Billing is self-serve via Stripe (see pricing page).
