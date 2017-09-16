using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }



        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            int attackType = 0;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                attackType = 3;
            else if (Input.GetKeyDown(KeyCode.Q))
                attackType = 1;
            else if (Input.GetKeyDown(KeyCode.E))
                attackType = 2;
            // Pass all parameters to the character control script.
            m_Character.Attack(attackType);
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
