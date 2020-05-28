# BasicFPController
Simple first-person controller for Unity. Includes proper jumping with minimal use of layers and proper crouch detection.

## Breakdown of scripts
1. Follow Player
    1. Attach to Empty Object MainCamera is attached to. Has Object follow player.
1. ImpCharacterController
    1. Attach to Player. Create new layer named Player and give it to the Player. Movement is transform-based. Jumping is rigidbody-based. Controls are hard-defined.
1. [SmoothMouseLook](https://wiki.unity3d.com/index.php/SmoothMouseLook)

