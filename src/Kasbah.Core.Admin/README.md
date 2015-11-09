# Kasbah.Core.Admin

This module provides an administrative interface to the core services.

 1. The content tree
 1. The document index
 1. The event bus

## Dependencies

 * `Kasbah.Core`
 * `Kasbah.Core.Events (+ .Redis)`
 * `Kasbah.Core.ContentTree (+ .Npgsql)`
 * `Kasbah.Core.Index (+ .Solr)`

## Usage

See `~/docker/README.md` about getting the kasbah/dev container running.

Startup the back-end from the project root.  This will run the API on port 5004 (by default).

    DB="Server=<docker-machine ip>;Port=50002;Database=postgres;User Id=postgres" nodemon -e cs,sql --exec "dnx -p src/Kasbah.Core.Admin kestrel"

Install the npm packages in `wwwroot`.

    cd wwwroot
    npm install

Then start the front-end from that directory.  This will build the webpack package and start hosting the React/redux SPA page at [http://localhost:3000/]

    npm start
