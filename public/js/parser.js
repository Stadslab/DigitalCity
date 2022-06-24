(function() {

	class Parser {

		constructor() {

			this.matrix = null;

			this.parse = function( data, scene ) {

				let meshes = [];

				// loop through all objects available
				for ( const objectId in data.CityObjects ) {

					// parse the geometry of this specificy object.
					const geom = this.parseObject( objectId, data );

					// create the material of the mesh
					const material = new THREE.MeshLambertMaterial();

					// set the color of the mesh (building)
					material.color.setHex(0xffcc0b);

					// construct the mesh using the material & the geometry that we've created.
					const mesh = new THREE.Mesh( geom, material );

					// store the object id in to the mesh name & enable shadows.
					mesh.name = objectId;
					mesh.castShadow = true;
					mesh.receiveShadow = true;

					// add the created mesh to our scene.
					scene.add( mesh );

					// push the mesh to our global mesh array for later use.
					meshes.push(mesh);

				}

				return meshes;

			}

			this.parseObject = function( objectId, json ) {

				// retrieve the object from the array
				const cityObject = json.CityObjects[ objectId ];

				// if object has no geometry available, return
				if ( ! ( cityObject.geometry &&
				  cityObject.geometry.length > 0 ) ) {

				  return;

				}

				// construct new geometry
				const geom = new THREE.Geometry();
				let vertices = [];

				// loop through all geoms inside the object.
				for ( let geom_i = 0; geom_i < cityObject.geometry.length; geom_i ++ ) {

				  // handle each geomtype accordingly.
				  const geomType = cityObject.geometry[ geom_i ].type;

				  // if type is solid, proceed with parsing the solid version
				  if ( geomType == "Solid" ) {

						const shells = cityObject.geometry[ geom_i ].boundaries;

						for ( let i = 0; i < shells.length; i ++ ) {

							this.parseShell( geom, shells[ i ], vertices, json );

						}

					} else if ( geomType == "MultiSurface" || geomType == "CompositeSurface" ) {

						const surfaces = cityObject.geometry[ geom_i ].boundaries;

						this.parseShell( geom, surfaces, vertices, json );

					} else if ( geomType == "MultiSolid" || geomType == "CompositeSolid" ) {

						const solids = cityObject.geometry[ geom_i ].boundaries;

						for ( let i = 0; i < solids.length; i ++ ) {

							for ( let j = 0; j < solids[ i ].length; j ++ ) {

								this.parseShell( geom, solids[ i ][ j ], vertices, json );

							}

						}

					}

				}

				// if matrix is specified, apply it.
				if ( this.matrix !== null ) {

					geom.applyMatrix4( this.matrix );

				}

				// compute the face normals & return the geometry
				geom.computeFaceNormals();

				return geom;

			}

			this.parseShell = function( geom, boundaries, vertices, json ) {

				// loop through all the boundaries
				for ( let i = 0; i < boundaries.length; i ++ ) {

					let boundary = [];
					let holes = [];

					for ( let j = 0; j < boundaries[ i ].length; j ++ ) {

						if ( boundary.length > 0 ) {

							holes.push( boundary.length );

						}

						const new_boundary = this.extractLocalIndices( geom, boundaries[ i ][ j ], vertices, json );
						boundary.push( ...new_boundary );

					}

					if ( boundary.length == 3 ) {

						geom.faces.push( new THREE.Face3( boundary[ 0 ], boundary[ 1 ], boundary[ 2 ] ) );

					} else if ( boundary.length > 3 ) {

						// create list of points
						let pList = [];
						for ( let k = 0; k < boundary.length; k ++ ) {

							pList.push( {
								x: json.vertices[ vertices[ boundary[ k ] ] ][ 0 ],
								y: json.vertices[ vertices[ boundary[ k ] ] ][ 1 ],
								z: json.vertices[ vertices[ boundary[ k ] ] ][ 2 ]
							} );

						}

						// get normal of these points
						const normal = this.get_normal_newell( pList );

						// convert to 2d (for triangulation)
						let pv = [];
						for ( let k = 0; k < pList.length; k ++ ) {

							const re = this.to_2d( pList[ k ], normal );
							pv.push( re.x );
							pv.push( re.y );

						}

						// triangulate
						const tr = earcut( pv, holes, 2 );

						// create faces based on triangulation
						for ( let k = 0; k < tr.length; k += 3 ) {

							geom.faces.push(
								new THREE.Face3(
									boundary[ tr[ k ] ],
									boundary[ tr[ k + 1 ] ],
									boundary[ tr[ k + 2 ] ]
								)
							);

						}

					}

				}

			}

			this.extractLocalIndices = function( geom, boundary, indices, json ) {

				let new_boundary = [];

				for ( let j = 0; j < boundary.length; j ++ ) {

					// the original index from the json file
					const index = boundary[ j ];

					// if this index is already there
					if ( indices.includes( index ) ) {

						const vertPos = indices.indexOf( index );
						new_boundary.push( vertPos );

					} else {

						// add vertex to geometry
						const point = new THREE.Vector3(
							json.vertices[ index ][ 0 ],
							json.vertices[ index ][ 1 ],
							json.vertices[ index ][ 2 ]
						);
						geom.vertices.push( point );

						new_boundary.push( indices.length );
						indices.push( index );

					}

				}

				return new_boundary;

			}

			this.get_normal_newell = function( indices ) {

				// find normal with Newell's method
				let n = [ 0.0, 0.0, 0.0 ];

				for ( let i = 0; i < indices.length; i ++ ) {

				  let nex = i + 1;

				  if ( nex == indices.length ) {

						nex = 0;

					}

				  n[ 0 ] = n[ 0 ] + ( ( indices[ i ].y - indices[ nex ].y ) * ( indices[ i ].z + indices[ nex ].z ) );
				  n[ 1 ] = n[ 1 ] + ( ( indices[ i ].z - indices[ nex ].z ) * ( indices[ i ].x + indices[ nex ].x ) );
				  n[ 2 ] = n[ 2 ] + ( ( indices[ i ].x - indices[ nex ].x ) * ( indices[ i ].y + indices[ nex ].y ) );

				}

				const b = new THREE.Vector3( n[ 0 ], n[ 1 ], n[ 2 ] );
				return ( b.normalize() );

			}

			this.to_2d = function( p, n ) {

				p = new THREE.Vector3( p.x, p.y, p.z );
				const x3 = new THREE.Vector3( 1.1, 1.1, 1.1 );
				if ( x3.distanceTo( n ) < 0.01 ) {

				  x3.add( new THREE.Vector3( 1.0, 2.0, 3.0 ) );

				}

				let tmp = x3.dot( n );
				let tmp2 = n.clone();
				tmp2.multiplyScalar( tmp );
				x3.sub( tmp2 );
				x3.normalize();

				let y3 = n.clone();
				y3.cross( x3 );

				let x = p.dot( x3 );
				let y = p.dot( y3 );

				const re = { x: x, y: y };

				return re;

			}

		}

	}

	window.Parser = Parser;

})();