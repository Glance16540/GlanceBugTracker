//VARIABLES 
var renderer, scene, windowHeight, windowWidth, aspectRatio, fieldOfView, near, far, camera, cube;

var Animation = {
    duration: 1.5,
    delay: .5,
    colors: {
        cube: 0xE0E0E0,
        plateform: 0x9E9E9E,
        ground: 0xf99d17,
        background: 0xffad19
    }
};

function getDegree(degree) {
    return degree * Math.PI / 180;
}

//INIT
function init() {
    newScene();
    newLights();
    newPlateform();
    newGround();
    newCube();
    render();
}

//SCENE
function newScene() {
    windowHeight = window.innerHeight;
    windowWidth = window.innerWidth;
    scene = new THREE.Scene();

    aspectRatio = windowWidth / windowHeight;
    factor = 130;
    near = 1;
    far = 10000;
    camera = new THREE.OrthographicCamera(windowWidth / -factor, windowWidth / factor, windowHeight / factor, windowHeight / -factor, near, far);
    camera.position.set(7, 5, 5);
    camera.lookAt(scene.position);

    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setClearColor(new THREE.Color(Animation.colors.background), 1);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    renderer.setSize(windowWidth, windowHeight);
    document.body.appendChild(renderer.domElement);

    window.addEventListener('resize', windowResize, false);
    function windowResize() {
        renderer.setSize(window.innerWidth, window.innerHeight);
        camera.left = -window.innerWidth / factor;
        camera.right = window.innerWidth / factor;
        camera.top = window.innerHeight / factor;
        camera.bottom = -window.innerHeight / factor;
        camera.updateProjectionMatrix();
    }
}

//LIGHTS
function newLights() {
    var light = new THREE.AmbientLight(0xFFFFFF, .9);
    var hemisphereLight = new THREE.HemisphereLight(0xFFFFFF, 0xFFFFFF, .1);
    var shadowLight = new THREE.DirectionalLight(0xFFFFFF, .1);
    shadowLight.position.set(10, 100, 30);
    shadowLight.castShadow = true;
    scene.add(light);
    scene.add(hemisphereLight);
    scene.add(shadowLight);
}

//GROUND
Ground = function () {
    this.mesh = new THREE.Object3D();
    var geometry = new THREE.PlaneGeometry(20, 20, 1, 1);
    var material = new THREE.MeshLambertMaterial({ color: Animation.colors.ground });
    var plane = new THREE.Mesh(geometry, material);
    plane.receiveShadow = true;
    this.mesh.add(plane);
};

function newGround() {
    ground = new Ground();
    ground.mesh.position.y = -2;
    ground.mesh.rotation.x = getDegree(-90);
    scene.add(ground.mesh);
}


//PLATEFORME
Plateform = function (posX, posY, posZ) {
    this.mesh = new THREE.Object3D();
    var geometry = new THREE.BoxGeometry(1, .2, 1);
    var material = new THREE.MeshLambertMaterial({ color: Animation.colors.plateform });
    this.plateform = new THREE.Mesh(geometry, material);
    this.plateform.castShadow = true;
    this.mesh.add(this.plateform);
};

var plateformee;
function newPlateform() {
    var plateformPosition = [[0, -1, .5], [0, -1, -.5], [-1, -1, -.5]];
    for (var pos in plateformPosition) {
        plateformee = new Plateform();
        plateformee.mesh.position.x = plateformPosition[pos][0];
        plateformee.mesh.position.y = plateformPosition[pos][1];
        plateformee.mesh.position.z = plateformPosition[pos][2];
        scene.add(plateformee.mesh);
    }
}


//CUBE
Cube = function () {
    this.mesh = new THREE.Object3D();
    var geometry = new THREE.BoxGeometry(1, 1, 1);
    var material = new THREE.MeshLambertMaterial({ color: Animation.colors.cube, transparent: true });
    this.cube = new THREE.Mesh(geometry, material);
    this.mesh.add(this.cube);
};

function newCube() {
    cube = new Cube();
    scene.add(cube.mesh);
    cube.cube.position.set(0, 2, .5);

    var tl = new TimelineMax({ repeat: -1, repeatDelay: Animation.delay });
    tl.set(cube.cube.material, { opacity: 0 });
    tl.to(cube.cube.position, 0.8, { y: -.4, ease: Bounce.easeOut });
    tl.to(cube.cube.scale, 0.8, { y: 1, ease: Bounce.easeOut }, "-=0.8");
    tl.to(cube.cube.material, 0.5, { opacity: 1 }, "-=0.8");
    tl.to(cube.cube.rotation, 0.8, { x: getDegree(-90) }, "+=0.2");
    tl.to(cube.cube.position, 0.3, { y: -.2 }, "-=0.8");
    tl.to(cube.cube.position, 0.8, { z: -0.5 }, "-=0.8");
    tl.to(cube.cube.position, 0.3, { y: -.4 }, "-=0.4");
    tl.to(cube.cube.rotation, 0.8, { y: getDegree(-90) });
    tl.to(cube.cube.position, 0.3, { y: -.2 }, "-=0.8");
    tl.to(cube.cube.position, 0.8, { x: -1 }, "-=0.8");
    tl.to(cube.cube.position, 0.3, { y: -.4 }, "-=0.4");
    tl.to(cube.cube.rotation, 0.8, { x: 0, ease: Power3.easeOut });
    tl.to(cube.cube.position, 0.8, { z: 0.8, ease: Power3.easeOut }, "-=0.8");
    tl.to(cube.cube.position, 0.60, { y: -4, ease: Power3.easeIn }, "-=0.80");
    tl.to(cube.cube.scale, 0.8, { y: 1.5 }, "-=0.5");
    tl.to(cube.cube.material, 0.25, { opacity: 0 }, "-=0.85");
    tl.timeScale(Animation.duration);
}

//RENDER
function render() {
    renderer.render(scene, camera);
    requestAnimationFrame(render);
}

init();