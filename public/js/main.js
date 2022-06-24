(async () => {

	$("#permissions").click(() => {

		$("#permissions").text('Laden..');

		// if the requestPermission function eixsts, call it
		if (typeof DeviceMotionEvent.requestPermission === 'function') {

			DeviceMotionEvent.requestPermission().then((state) => {

				// check if user granted the permissions
		        if (state !== 'granted') {
		        	alert('Request to access the orientation was rejected');
		        }

		        $("#permissions").attr('disabled', 1);
				$("#permissions").text('Geef Toegang');

		    }).catch((err) => {
		    	$("#permissions").attr('disabled', 0);
		    	$("#permissions").text('Geef Toegang');
		    });

		}

	});

	$("#preplaunch").click(() => {
		$("#info").hide();
		$("#prepinfo").show();
	})

	$("#launch").click(async () => {

		if($("#launch").text() == 'Applicatie wordt gestart..')
			return;

		$("#launch").text('Applicatie wordt gestart..');

		// load al available zones on launch.
		await new Promise((resolve, reject) => {

			// load all files
			zones.forEach(async (zone, index, array) => { 

				await zone.load(Engine.scene); 

				if(index == array.length - 1)
					return resolve();

			});

		});

		// once the zones are loaded in, show the canvas where we draw on.
		$("#preplaunch").hide();
		$("#canvas").show();

		// enable the orientation system
		Engine.handleOrientation();


	});

	// store all files needed for loading
	let zones = [
		new Zone3D('city/3344.json')
	];

	// generate our scene
	await Engine.generateScene();

	// generate the map
	Engine.generateMap();

	// generate sky & lighting
	Engine.generateSky();
	Engine.generateLighting();

})();

