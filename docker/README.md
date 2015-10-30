Docker images, layered to produce single image for running the whole system.  Useful for development.

---

Use the kasbah/dev image for running tests.

    docker run -d -p 50001:6379 -p 50002:5432 -p 50003:8983 kasbah/dev

This will run a docker instance, exposing Redis on port 50001, PostgreSQL on port 50002 and Solr on port 50003.  The database will have the schema installed, but no data.

To run the Kasbah.Core.ContentTree.Npgsql tests, find the IP address of your docker machine (if using Docker Toolbox) by running

    docker-machine ip default

Then from the root of the project run

    DB="Server=<docker-machine ip>;Port=50002;Database=postgres;User Id=postgres" sh run-tests.sh content-tree-npgsql
