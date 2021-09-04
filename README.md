# Using Kerbal Renamer
There are 55+ cultures in `kerbalrenamer.cfg` with firstname for both binary genders. Near the bottom of this file, there are a number of profiles that defines the cultural mix that can be used to generate names for a program.

Due to a quirk with KSP, the initial 4 kerbals are generated from the profile `1951`. It is made of nations with some kind of rocket research expertise in 1951. You may edit your own copy if you don't like this mixture. 

## Simple use
Select ONE profile from the Settings section for Kerbal Renamer. Whilst it is possible to select more than one (a UI issue), only one will be used. 

## Custom use
It is possible to define your own mixture of culture by editing the weights in the `CUSTOM` profile, then selecting this profile in the relevant Settings section. 

# Adding cultures
You are welcome to add cultures to `kerbalrenamer.cfg` by copying another culture, and editing the FFIRSTNAME1, MFIRSTNAME1, LASTNAME1, optionally FLASTNAME1 (last name for women). Such edits can be shared with the community through a Pull request. If the culture places surnames before first names, use the config node value `pattern = LF` as with `China` and `Hungary`.

# Representation
The cultural coverage in `kerbalrenamer.cfg` is derived from frequency dataset and largely covers Northern hemisphere nationality. The nations relevant to the 1951+ space race are covered. There are also many nations that are not particularly relevant. This being said, there are many missing nations. The choice to include/exclude was pragmatic and based on available data. It certainly is not a thorough enthographic study. 
Finally, KSP uses a binary gender system (M/F). We acknowledge that this is an oversimplification. 

