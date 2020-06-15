This is a simple tool based on the ANT+ dynamic library that calculate the power you produce on 
indorr-trainers. The program works any usb ANT+ dongle. It recieves data from speed and cadence 
sensor, than it calculates the power using values that you can collect from the graph printed on the 
trainers box or on the trainer itself. These values can be inserted in the trainersList.xml file 
so you can select it from the programs gui. The calculated power is displayed on-screen but can be 
recieved and displayed on any cyclo-computer (like a Garmin) that supports ANT+ sensors. This can be 
done by scanning for all power sensor from the cyclo-computer.
The power is calculated using a linear function, so is accurate only under 40 km/h, but i'm working
on creating an exponential function for more precise power calculation.
I'm new to c#, so feel free to mention anything that can be done in a better/more efficent way.