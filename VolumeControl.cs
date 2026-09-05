using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    void Awake()
    {
        // ‘S‘Ì‚Ì‰¹—Ê‚ð 0.6 (60%) ‚É‰º‚°‚ÄWebGL‚Å‚Ì‰¹Š„‚ê‚ð–h‚®
        AudioListener.volume = 0.6f;
    }
}
