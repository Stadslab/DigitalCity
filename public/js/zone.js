(function() {

	class Zone3D {

		constructor(file) {

			// initialize the parser that will be used for this specific zone.
			this.parser = new Parser();
			
			this.objects = null;
			this.file = file;

		}

		async load(scene) {

			let _this = this;

			return new Promise((resolve, reject) => {
				
				// fetch file from path
				$.getJSON(_this.file, (data) => {

					// store in object
					_this.objects = data;

					// process the objects & pass to scene
					let object = _this.processObjects(scene); 

					Engine.meshes.push(object);

					return resolve(object);

				});

			});

		}

		processObjects(scene) {

			// loop through all available vertices 
			for(let i = 0; i < this.objects.vertices.length; i++) {

				// if the transform property is defined, apply the formula that we retrieved from the cityjson website.
				if(this.objects.transform) {
					for(let y = 0; y < 3; y++) {
						this.objects.vertices[i][y] = this.objects.vertices[i][y] * (this.objects.transform.scale[y]);
						this.objects.vertices[i][y] = this.objects.vertices[i][y] + this.objects.transform.translate[y];
					}
				}

				// convert the vertex coordinates from Amersfoort to WGS84.
				let coordsWsg = Utils.toWSG(this.objects.vertices[i][0], this.objects.vertices[i][1]);

				// convert the WGS coordinates to 3D coordinates.
				let coordsBuilding = Geo.UnitsUtils.datumsToSpherical(coordsWsg[0], coordsWsg[1]);

				// store 3D coordinates back in to our vertex array for later use.
				this.objects.vertices[i][0] = coordsBuilding.x;
				this.objects.vertices[i][1] = this.objects.vertices[i][2] - 2;
				this.objects.vertices[i][2] = -coordsBuilding.y;
			}

			return this.parser.parse(this.objects, scene);
		}

	}

	window.Zone3D = Zone3D;

})();
