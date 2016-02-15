# Kasbah.Core

Kasbah is a framework that can be used whenever an application requires data to be stored in a tree structure that can be easily queried and searched.

This core framework is no use on its own.  It simply provides a base to work with.

The inspiration for this project came from frustrations with many modern content management systems (CMSs).  The initial goal was to build something that ran cross-platform, performed well and was easy to develop with.

The tree structure is simple.  There are `nodes` and there are `node versions`.  Nodes define the structure of the tree, where node versions store the content of each node in the tree.  Nodes can specify their active version (or leave it blank).

This core project contains services for interacting with data at a low level and a broker to simplify data access.

The services provided by the core are:

 * **ContentTreeService** *Kasbah.Core.ContentTree* provides an interface for interacting with the core datastore that holds the current tree structure along with all versions of data stored in the tree
 * **IndexService** *Kasbah.Core.Index* provides an interface for searching the currently active node data
 * **EventService** *Kasbah.Core.Events* provides a messaging bus for communication throughout the system
 * **ContentBroker** *Kasbah.Core.ContentBroker* ties all the core services together

Most interactions should go through the **ContentBroker**, very rarely should you need to access the underlying services.

Core services are based on abstract providers so the core system is not tied to any specific software stack.  For example, the **EventService** contains two implementations in the core, one for in-memory communication and one for pub-sub communication via Redis.

Implementations of providers shipped with the core are:

 * ContentTreeService
  * Npgsql - for working with Postgresql databases
 * IndexService
  * Solr - Best for distributed systems where the application can be running on multiple hosts
  * Lucene (incomplete) - Can be used for simple applications
 * EventService
  * InProc - For inner-process communication
  * Redis - For distributed pub communcation

## Getting started

Ensure you have the DNX environment setup.  See here for instructions: https://github.com/aspnet/Home

Restore the dnu packages

    dnu restore

Start running the tests.  They'll automatically re-run on code changes.

    sh run-tests.sh
