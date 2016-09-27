For assets other than those from 3D models, folders are broken up by what the item is instead of what the asset is. For example this means that both 
the material and texture for the Brown Splatter effect are in the same sub folder of the Effects subfolder. This is because it is more common 
to want to reference the texture while working on the material than it is to want to be making changes to all of the materials or all of 
the textures at once. It is not likely that two devs will be working on the same feature at the same time so theoretically one dev could be working 
on the Brown Splatter while another works on the Teleport Reticle and neither will cause changes to the other. If all the materials are in 
one folder and all the textures in another then it is more likely a merge conflict will occur.

For materials and textures that are specifically for 3D models created in Cinema 4D, go to that models respective sub folder under Models.