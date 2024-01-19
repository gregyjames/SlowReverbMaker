[![.NET](https://github.com/gregyjames/SlowReverbMaker/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gregyjames/SlowReverbMaker/actions/workflows/dotnet.yml)

# SlowReverbMaker

I wanted to play around with some audio programmatically, and what better way to do that than by making slowed reverbed song remixes?

The premise for this is fairly straightforward:
1. Audio file to NAudio samples
2. Pitch shift (essentially the same as changing tempo)
3. Custom Echo provider
4. Custom Reverb provider
5. High pass filter to clean up some of the muddy low sub frequencies from all the reverb.

I am not an audio engineer, so if you have ___any___ suggestions on improving the formulas or workflow changes, by all means, go ahead!! Otherwise, feel free to check it out or play around with the settings to match your taste. 

Unfortunately, NAudio uses Media Foundation (windows only) so this experiment is *NOT* cross-platform.
