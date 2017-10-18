This is a really rudimentary simulator for a simple car with customisable physics which at the moment are kind of broken.

Controls:
1. WASD or arrow keys to accelerate reverse and steer
2. Left Ctrl to brake/right the car if flipped

How to run:
Option 1 (play only):
-If you just want to play the sim  with the default values, for whatever reason, double-click the EcoCarSim.exe at the top of the project hierarchy

Option 2 (tweek mode):
-This option is for tweeking physics values and to just edit the sim in general which requires you to have Unity5 installed.
 1.Install Unity5
 2.Open the EcoCarSim folder in Unity
 3.From here you can freely edit the sim however you like.
 4.To tweek car values...
    i)Click on the EcoBrick gameobject in the scene editor. It should be highlighted blue text with an arrow next to it.
    ii)In the inspector on the right you should see a script called SimpleCarController with a bunch of public values like 'Max Torque, Ideal RPM'
       and stuff like that. You can edit these values to change how the car works. RPM mostly controls top speed and Torque controls
       the acceleration/horsepower and stuff like that. Make sure to change the 'Break Torque' proportionally with the 'Max Torque'.
    iii)Uncollapse 'axelinfos' above 'Max Torque' to change the drive train, i.e: front wheel, rear wheel or all wheel drive, by ticking motor
        for each wheel set. One could select which wheels steer here as well but I don't recommend it. The code I wrote doesn't really allow
        4 wheel steering...
    iv)The source code can be accessed in the folder labelled Scripts.

 5. To tweak the tire physics...
     i)Uncollapse the EcoBrick gameobject in the scene editor
     ii)Uncollapse all the wheel colliders labelled FL,FR,BR,BL so that the cylinder child of each collider is visible. CTRL click all the colliders(FL,FR,BR,BL)
        so they are all selected at once.
     iii)Now, you can edit the tire physics values on all the tires simultaneously.
     iv) To be honest, I have absolutely no notion whatsoever of what these numbers do. The ones in there were obtained through pure trial
         and error and might as well be random. All I can tell is that the current values equate to a really low level of friction on the tires causing them to
         drift maniacly at high RPMs. But the current car values make for a fairly reactive, slightly realistic car.
 6. Run the scene by pressing the big, obvious play button at the top of the Unity window. The car will be controlled the same as in Option 1.

Know Issues:
-Due to the low friction the tires RPM varies really drastically from frame to frame making the speed text mostly illegible. Although if you are 
 flying through the air it will work perfectly. The calculation in the code is still working as expected despite this behaviour.
-Drastically increasing the torque makes the car do some pretty fuckin' gnarly drifts; like some The Fast and The Furious level shit. Probably also mostly due to the
wonky tire physics.
-Increasing the numbers for the wheel colliders too much will change this into a EcoRocket simulator and the car will do its best Team Rocket impression by blasting
off at near escape velocity. From what I can tell the slip numbers should be kept relatively small with a proportonially small difference between the Asymptote and Value
numbers.
-Breaking with CTRL is the only real way to slow the car. It doesn't stop on its on cause its 1500 kg brick with greased up wheels and trying to use reverse will just turn
you into Ken Block.