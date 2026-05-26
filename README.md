# DeadlockTiers

A REST API that calculates a live hero tier list for Valve's Deadlock.

## What it does

Fetches hero win/loss statistics from the Deadlock community API, scores each hero using a net win rate formula, and ranks them into tiers (S through F) relative to the field.

**Endpoint:** `GET /heroes` — returns all heroes with their tier and score, sorted strongest to weakest.

## Scoring

Each hero is scored as `(wins - losses) / matches`. Tiers are assigned based on deviation from the mean score across all heroes, with a 5% threshold between each tier.

## Stack

- .NET 10, ASP.NET Core Minimal APIs
- External data: [deadlock-api.com](https://deadlock-api.com)

## Run locally

```bash
git clone https://github.com/GPGCode/DeadlockTierAPI
cd DeadlockTierAPI
dotnet run
```

Hit `http://localhost:5000/` — no setup, no config, no database.
