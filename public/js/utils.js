(function() {

	class Utils {

		constructor() {

			this.toWSG = function(x, y) {

				const x0 = 155000;
				const y0 = 463000;

				const phi0 = 52.15517440;
				const lam0 = 5.38720621

				let pqk = [
					{ p: 0, q: 1, k: 3235.65389 },
			        { p: 2, q: 0, k: -32.58297 },
			        { p: 0, q: 2, k: -0.24750 },
			        { p: 2, q: 1, k: -0.84978 },
			        { p: 0, q: 3, k: -0.06550 },
			        { p: 2, q: 2, k: -0.01709 },
			        { p: 1, q: 0, k: -0.00738 },
			        { p: 4, q: 0, k: 0.00530 },
			        { p: 2, q: 3, k: -0.00039 },
			        { p: 4, q: 1, k: 0.00033 },
			        { p: 1, q: 1, k: -0.00012 } 
				];

				let pql = [
					{ p: 1, q: 0, l: 5260.52916 }, 
			        { p: 1, q: 1, l: 105.94684 }, 
			        { p: 1, q: 2, l: 2.45656 }, 
			        { p: 3, q: 0, l: -0.81885 }, 
			        { p: 1, q: 3, l: 0.05594 }, 
			        { p: 3, q: 1, l: -0.05607 }, 
			        { p: 0, q: 1, l: 0.01199 }, 
			        { p: 3, q: 2, l: -0.00256 }, 
			        { p: 1, q: 4, l: 0.00128 }, 
			        { p: 0, q: 2, l: 0.00022 }, 
			        { p: 2, q: 0, l: -0.00022 }, 
			        { p: 5, q: 0, l: 0.00026 }
				];

				let dx = 1e-5 * ( x - x0 );
				let dy = 1e-5 * ( y - y0 );

				let phi = phi0;
				let lam = lam0;

				pqk.forEach((a) => {

					phi += a.k * dx**a.p * dy**a.q / 3600;

				});

				pql.forEach((a) => {

					lam += a.l * dx**a.p * dy**a.q / 3600;

				});

				return [phi, lam];

			}

			this.getLocation = async function() {

				return new Promise((resolve, reject) => {

					// fetch the current device position.
			        navigator.geolocation.getCurrentPosition((data) => {

			        	// return the retrieved data in the promise.
			            return resolve(data);
			            
			        }, (err) => {

			        	// if an error occured, show the nolocation div.
			            document.querySelector('#nolocation').style.display = 'flex';
			            document.querySelector('#canvas').style.display = 'none';

			            return reject(err);
			        });
			        
			    });

			}

			this.isIOS = function() {
			  return [
			    'iPad Simulator',
			    'iPhone Simulator',
			    'iPod Simulator',
			    'iPad',
			    'iPhone',
			    'iPod'
			  ].includes(navigator.platform)
			  // iPad on iOS 13 detection
			  || (navigator.userAgent.includes("Mac") && "ontouchend" in document)
			}

		}

	}

	window.Utils = new Utils();

})();