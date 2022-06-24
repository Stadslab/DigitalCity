# V-City

V-City is a simple project that consists of 2 parts. The first part is a 3D world of Rotterdam created with Three.JS. To achieve this it uses CityJSON data for the buildings. The second part of this project is a data converter that takes CityGML data and turns it into CityJSON data.
## Getting started

### Prequesties
No additional requirements are needed for the mobile application.

The converter requires the following programs:

- [Java JDK](https://www.oracle.com/java/technologies/downloads/#jdk18-windows)
- [Python](https://www.python.org/downloads/)

During the installation of both programs, it is important that the [Java path](https://mkyong.com/java/how-to-set-java_home-on-windows-10/) and [Python path](https://www.javatpoint.com/how-to-set-python-path) are set.

### Installation

To pull the project, run the following command in your terminal (make sure your SSH keys are set):
```bash
git clone git@github.com:HRO-1001086/ThreeJS-CityJSON.git
```
## How to use - 3D world

To open the 3D world of Rotterdam you need to navigate to the public folder and open index.html. It is possible to host this on a webserver by transferring the whole folder public

## How to use - Converter

To use the data converter open the python file `dataconverter/converter.py`. Put all CityGML files in the dataconverter directory in a separate folder and indicate in the console that you want to convert this folder. A new folder will be created with CityJSON files.
## Features

- Display Rotterdam in 3D.
- Convert CityGML to CityJSON.
- Retrieve location and compass from device.
## Running Tests

To run tests, run the following command inside cityjsonconverter:

```bash
  python tests/test.py
```

