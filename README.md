# SMILEI Vokaturi plugin
This is a plugin for SMILEI Core that lets you use the Vokaturi SDK to read emotion data from audio.

## Getting Started
The package can be installed by adding this git repository to the Packages/manifest.json file along with SMILEI Core.

```
  "dependencies": {
    "nl.ixdfontysict.smilei.core" : "https://git.fhict.nl/I875317/smilei-core.git#0.0.2"
    "nl.ixdfontysict.smilei.vokaturi" : "https://git.fhict.nl/I872272/smilei_vokaturi.git#0.0.2"
  }
```

## How to use

### VokaturiMixerAsset
You can create a new VokaturiMixerAsset by selecting Create->Vokaturi->Mixer. The VokaturiMixerAsset holds a VokaturiMixer which needs an Emotion as defined by the Vokaturi API, and a VokaturiSampler.
A VokaturiMixer needs to be 'activated' to tell Vokaturi you are reading data and deactivated when it is no longer needed. Do this by calling the Register() and Unregister() methods respectively.

### VokaturiSampler
Currently there is only one sampler type: VokaturiSampler. You can create a sampler by selecting Create->Vokaturi->Sampler in the project window.
For now the VokaturiSampler is always active. Usually you would have a single sampler for a single audio source.
A VokaturiSampler samples an audio buffer at a defined sample interval. It needs an AudioBuffer.

### AudioBuffer
You can create an AudioBuffer by selecting Create->Vokaturi->AudioBuffer in the project window.

### MicrophoneToBuffer and AudioClipToBuffer
These MonoBehaviours let you write audiodata from the microphone or from an audioclip to an AudioBuffer asset for analysis by Vokaturi.

### VokaturiMixerRegistrar
This Monobehaviour calls Register() and Unregister() on VokaturiMixerAssets.

## Debugging

### VokaturiOutputLogger
Outputs VokaturiMixer data as text. 


## Contributions
This project is being developed by [Fontys IXD](https://www.ixdfontysict.nl/).