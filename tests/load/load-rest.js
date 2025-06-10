import { check, sleep } from 'k6';
import http from 'k6/http';
import { uuidv4, randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';


const params = {
    headers: {
        'Content-Type': 'application/json',
        'X-Tenant': uuidv4(),
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6ImZ1YXlxbkNRb0F2OWRCMHMxTjJPSTlZUkFMeElVQlg1bGtReHZxall1VVUiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJhZTViZDQ5Mi1hOWE4LTQ0NjItOTE1My1hNzFmOTYwZWQyNjkiLCJpc3MiOiJodHRwczovL2NvZGVkZXNpZ25wbHVzZGV2ZWxvcG1lbnQuYjJjbG9naW4uY29tL2ZiMjNkM2U4LTdkZTctNDU1NC05ZDczLTc4ZTk5MDRhMjQzZi92Mi4wLyIsImV4cCI6MTc0ODk5MTgxOCwibmJmIjoxNzQ4OTg4MjE4LCJzdWIiOiI0N2JiZDcyMi1lMGI3LTQzMGUtYWViMi02MGM1ZjE4Yjg5ZWIiLCJvaWQiOiI0N2JiZDcyMi1lMGI3LTQzMGUtYWViMi02MGM1ZjE4Yjg5ZWIiLCJuYW1lIjoiV2lsem9uIExpc2Nhbm8iLCJnaXZlbl9uYW1lIjoiV2lsem9uIiwiZmFtaWx5X25hbWUiOiJMaXNjYW5vIiwidGlkIjoiZmIyM2QzZTgtN2RlNy00NTU0LTlkNzMtNzhlOTkwNGEyNDNmIiwibm9uY2UiOiIzYTgwMTk5ODY5NDQ5ZTM0ZDcxM2M4MjhhOWQ3MTYxZTM2d1BoWU9pcyIsInNjcCI6InJlYWQiLCJhenAiOiJhZTViZDQ5Mi1hOWE4LTQ0NjItOTE1My1hNzFmOTYwZWQyNjkiLCJ2ZXIiOiIxLjAiLCJpYXQiOjE3NDg5ODgyMTh9.LLKymxWcfR1SdpqB1nLxokXSujJz6pzU4nRk6jXxuUhDuRY3PexBA_3wcYpnipUFEEs5eu2WzfA0nS4C63LriL1VtsW7V1kUe73ggnpGTKdoHYDpg7pFiwiMXzMd-HKldNtw3siJpzg1SWHi5-e6Iv85-aGAQugym0ydJyu5NqMKjFbpm-3aUbz21hig9jKqNTkVxjFKygsZs0BDvlIkSYbkIqVydswcPXq3GVZlFGT-agYRxdbnAwmRGeuLcLlZlmSUqtYDvmrNYtud6DXs23f91Li8ythDiAocArT5zr6TCUEjtIbcL97bQvMYOqsSmVEz2X0JHZ-faOAPJRMacQ'
    },
};

export const options = {
    discardResponseBodies: true,
    scenarios: {
        contacts: {
            executor: 'constant-arrival-rate',
            duration: '600s',

            // How many iterations per timeUnit
            rate: 50,

            // Start `rate` iterations per second
            timeUnit: '1s',

            // Pre-allocate 2 VUs before starting the test
            preAllocatedVUs: 100,

            // Spin up a maximum of 50 VUs to sustain the defined
            // constant arrival rate.
            maxVUs: 5000,
        },
    },
};

export default async function () {

    const order = JSON.stringify({
        "id": uuidv4(),
        "name": `Role ${randomIntBetween(1, 1000)}`,
        "description": `Description for role ${randomIntBetween(1, 1_000_000)}`,
    });

    let response = await http.asyncRequest('POST', 'https://services.codedesignplus.com/ms-roles/api/role', order, params);

    check(response, {
        'is status 204': (r) => r.status === 204,
    });

    sleep(1);
}