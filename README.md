# MechShaker

MechShaker generates bass shaker FX in real time for [MechWarrior 5: Mercenaries](https://mw5mercs.com/) - this can be used to drive Buttkickers, Daytons, or any tactile tranducer or force feedback device that accepts an audio signal input.

This repository contains the user interface and audio signal generation parts of the project.

## Features
* Fast response times to in-game events - sub 3ms in most cases
* Real time effects generated using weapon and mech stats - bigger cannons hit deep, assaults hit harder, stomp bigger, and land louder
* All weapon FX are covered including AMS, ACs, rifles, flamers, lasers, machine guns, missiles, and melee swings/hits
* Footsteps, jumpjets, landing impacts and torso twist FX
* Incoming damage and part destruction FX
* Dropship landing sequence and power up/down FX
* All effects are configurable with simple volume and frequency changes, or by enabling advanced mode there's access to over 200 parameters to fine tune each effect

## Download
The download and instructions for installing and using MechShaker are available on NexusMods here: [MechShaker](https://www.nexusmods.com/mechwarrior5mercenaries/mods/1029)

## How It Works
* **MechShakerRelay**, a blueprint mod for the game, gathers on-tick data and listens for a variety of in game events, packages up necessary details, and calls a parameterised OnTelemetry event that performs no immediate actions
* **MechShakerBridge**, a C++ plugin, is injected into the game using one of two methods. 
  * If using [MechWarriorVR](https://www.nexusmods.com/mechwarrior5mercenaries/mods/1009), as a [UEVR](https://github.com/praydog/UEVR) plugin (plugin source available [here](https://github.com/sicsix/MW5-UEVR-Plugins))
  * Otherwise, as a plugin for [UnrealModLoader](https://github.com/RussellJerome/UnrealModLoader)
* **MechShakerBridge**, when using the UEVR plugin, hooks directly into the OnTelemetry blueprint event. It then writes this telemetry data out to a memory mapped file
* **MechShaker** regularly reads this memory mapped file and generates real time bass shaker FX based on the data provided, and writes the FX to the selected audio output