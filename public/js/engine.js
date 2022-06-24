(async function() {

	async function loadCachedScene() {

		return new Promise((resolve, reject) => {

			$.getJSON('cache/scene.json', (data) => {

				return resolve(data);

			});

		});

	}

	class Engine {

		constructor() {

			// used to store actual GPS data.
			this.position = {
				latitude: 0.0,
				longitude: 0.0,
				verticalHeading: 0.0,
				horizontalHeading: 0.0,
				accuracy: 0.0
			}

			// used to store buildings
			this.meshes = [];

			// create clock to be used for controls
			this.clock = new THREE.Clock();

			// get canvas element
			this.canvas = document.getElementById("canvas");

			// create renderer
			this.renderer = new THREE.WebGLRenderer({
				canvas: canvas,
				antialias: true
			});

			// set pixel ratio
			this.renderer.setPixelRatio( window.devicePixelRatio );

			// create main camera & controls for the camera
			this.camera = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 0.1, 500000);

			// create the controls
			this.controls = new THREE.FirstPersonControls(this.camera, this.canvas);

			// set the speed of the controls.
			this.controls.lookSpeed = 0.1;

			// disable the flying capability
		    this.controls.noFly = true;

		    // allow the controls to be controlled vertically.
		    this.controls.lookVertical = true;

		    let _this = this;

		    // replace onresize callback with our custom one
		    document.body.onresize = this.resize.bind(this);

			// call resize callback manually once
			document.body.onresize();
		}

		async generateScene() {

			// create main scene
			this.scene = new THREE.Scene();

			// call our main render function after scene has been generated
			this.render();
		}

		render() {

			let delta = this.clock.getDelta();

			// get the animation frame
			requestAnimationFrame(this.render.bind(this));

			// call the webGL render callback
			this.renderer.render(this.scene, this.camera);

			// update our controls
			this.controls.update(delta);

		}

		resize() {

			// handle the resizing of the window accordingly
			let width = window.innerWidth;
			let height = window.innerHeight;

			this.renderer.setSize(width, height);

			this.camera.aspect = width / height;

			this.camera.updateProjectionMatrix();
			this.controls.handleResize();

		}

		generateMap() {

			// set our map provider to be OSMP
			let provider = new Geo.OpenStreetMapsProvider();

			// create the map & add it to our scene
			let map = new Geo.MapView(Geo.MapView.PLANAR, provider, provider);

			// add the map to our scene
			this.scene.add(map);
		}

		generateLighting() {

			// create ambient lighting
			this.scene.add(new THREE.AmbientLight(0xb1b1b1));

			// create directional lighting
			let directional = new THREE.DirectionalLight(0xACB0B6);
			directional.position.set(100, 10000, 700);
			
			// add the lighting to our scene
			this.scene.add(directional);

		}

		generateSky() {

			// Add Sky
			let sky = new THREE.Sky();
			sky.scale.setScalar(1e8);

			// GUI
			let effectController = {
				turbidity: 0,
				rayleigh: 0.5,
				mieCoefficient: 0.005,
				mieDirectionalG: 0.7,
				inclination: 0.48,
				azimuth: 0.25,
				exposure: 0.5
			};

			const uniforms = sky.material.uniforms;
			uniforms["turbidity"].value = effectController.turbidity;
			uniforms["rayleigh"].value = effectController.rayleigh;
			uniforms["mieCoefficient"].value = effectController.mieCoefficient;
			uniforms["mieDirectionalG"].value = effectController.mieDirectionalG;

			let theta = Math.PI * (effectController.inclination - 0.5);
			let phi = 2 * Math.PI * (effectController.azimuth - 0.5);

			let sun = new THREE.Vector3();
			sun.x = Math.cos(phi);
			sun.y = Math.sin(phi) * Math.sin(theta);
			sun.z = Math.sin(phi) * Math.cos(theta);
			uniforms["sunPosition"].value.copy(sun);

			// add the sky to our scene.
			this.scene.add(sky);

		}

		setPosition(lat, lon, height) {

			// convert coordinates to 3D coordinates.
			let coords = Geo.UnitsUtils.datumsToSpherical(lat, lon);

			// set camera to location
			this.camera.position.x = coords.x;
		    this.camera.position.y = height;
		    this.camera.position.z = -coords.y;
		}

		handleOrientation() {
			
			let _this = this;

			// if IOS, use a different listener
			if (Utils.isIOS()) {
   				window.addEventListener('deviceorientation', manageCompass);
			} else {
			    window.addEventListener("deviceorientationabsolute", manageCompass, true);
			}

			function manageCompass(event) {

				let absoluteHeading = 0.0;

				// if webkitCompassHeading exists, use it
			    if (event.webkitCompassHeading) {

			        absoluteHeading = event.webkitCompassHeading + 180;

			        _this.position.horizontalHeading = -absoluteHeading;

			    } else {
			    	// fine tune the heading using the compassHeading function to avoid the jitter effect.
			        absoluteHeading = 180 - compassHeading(event.alpha, event.beta, event.gamma);

			        // store our horizontal heading in our position object.
			        _this.position.horizontalHeading = absoluteHeading;
			    }

			    // store our vertical heading in our position object.
			    _this.position.verticalHeading = event.beta - 90;

				// store both headings in our camera class
			    _this.controls.custompos.clon = _this.position.horizontalHeading;
			    _this.controls.custompos.clat = _this.position.verticalHeading;

			}

			const compassHeading = (alpha, beta, gamma) => {

			    // Convert degrees to radians
			    const alphaRad = alpha * (Math.PI / 180);
			    const betaRad = beta * (Math.PI / 180);
			    const gammaRad = gamma * (Math.PI / 180);

			    // Calculate equation components
			    const cA = Math.cos(alphaRad);
			    const sA = Math.sin(alphaRad);
			    const cB = Math.cos(betaRad);
			    const sB = Math.sin(betaRad);
			    const cG = Math.cos(gammaRad);
			    const sG = Math.sin(gammaRad);

			    // Calculate A, B, C rotation components
			    const rA = - cA * sG - sA * sB * cG;
			    const rB = - sA * sG + cA * sB * cG;
			    const rC = - cB * cG;

			    // Calculate compass heading
			    let compassHeading = Math.atan(rA / rB);

			    // Convert from half unit circle to whole unit circle
			    if(rB < 0) {
			        compassHeading += Math.PI;
			    }else if(rA < 0) {
			        compassHeading += 2 * Math.PI;
			    }

			    // Convert radians to degrees
			    compassHeading *= 180 / Math.PI;

			    return compassHeading;
			};

			navigator.geolocation.watchPosition((pos) => {

				// store lat & long
				_this.position.latitude = pos.coords.latitude;
				_this.position.longitude = pos.coords.longitude;

				// convert WSG coordinates to world coordinates
				let coords = Geo.UnitsUtils.datumsToSpherical(_this.position.latitude, _this.position.longitude);

				// set camera to location
				this.camera.position.x = coords.x;
			    this.camera.position.y = 2;
			    this.camera.position.z = -coords.y;

				// store accuracy
				_this.position.accuracy = pos.coords.accuracy;

			}, (err) => {

				alert(err);

			}, { 
				enableHighAccuracy: true, 
				maximumAge: 10e3, 
				timeout: 20e3 
			});
		}
	}

	window.Engine = new Engine();

})();