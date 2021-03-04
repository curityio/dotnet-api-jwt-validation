Validate JWTs in .NET
=============================================

.. image:: https://img.shields.io/badge/quality-experiment-red
    :target: https://curity.io/resources/code-examples/status/

.. image:: https://img.shields.io/badge/availability-source-blue
    :target: https://curity.io/resources/code-examples/status/

This repository contains example code on how to use JWTs for authorization in .NET APIs. This will be managed using the `.NET Core Security Framework <https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-5.0/>`_. This allows for easy annotation of an endpoint to specify authorization policies.

::

    [HttpGet("developer")]
    [Authorize(Policy = "developer")]
    public IActionResult Developer()
    {
        ...
    }


This repository contains the result of the `Securing .NET Core API with JWT <https://curity.io/resources/tutorials/howtos/writing-apis/dotnet-api/>`_ article. That article contains more in-depth information about the source code.

Run the example
~~~~~~~~~~~~~~~~~~~

Update the ``appsettings.json`` according to the article.

Then run

:: 

    dotnet restore
    dotnet run

Contributing
~~~~~~~~~~~~

Pull requests are welcome. To do so, just fork this repo, and submit a pull request.

License
~~~~~~~

The files and resources maintained in this repository are licensed under the `Apache 2 license <LICENSE>`_.

More Information
~~~~~~~~~~~~~~~~

Please visit `curity.io <https://curity.io/>`_ for more information about the Curity Identity Server.

Copyright (C) 2021 Curity AB.

