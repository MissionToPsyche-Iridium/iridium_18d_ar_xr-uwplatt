/**
 * ARPage.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: This React component renders a full-screen augmented reality (AR) experience
 *          using an embedded iframe pointing to an external AR site.
 * Date Written: May 14, 2025
 */

import React from 'react';

/**
 * ARPage Component
 * 
 * This component embeds an external AR experience using an iframe that covers the entire
 * visible portion of the screen (below the navbar). It optionally supports a back button
 * callback handler, which is currently commented out.
 * 
 * Props:
 * - onBack: Function (optional) - A callback function to be called when the back button is pressed.
 */
const ARPage = ({ onBack }) => {
  return (
    <div style={styles.container}>
      <iframe
        title="AR Page"
        src="https://missiontopsyche-iridium.github.io/iridium_18d_ar_xr-uwplatt_unity/"
        style={styles.iframe}
        allow="camera; microphone" // Grants access to the user's camera and microphone for AR
      />
      {/* Uncomment the following block to enable the "Back" button */}
      {/* 
      <button onClick={onBack} style={styles.backButton}>
        Back
      </button> 
      */}
    </div>
  );
};

/**
 * Inline styles used to layout and style the AR page and optional button.
 */
const styles = {
  container: {
    position: 'fixed',     // Ensures the container stays in place
    top: '56px',           // Leaves space for the fixed navbar (typically 56px height)
    left: 0,
    width: '100%',
    height: '93%',         // Covers the remaining screen below the navbar
    zIndex: 1,
    overflow: 'hidden',    // Prevents scrollbars from appearing
  },
  iframe: {
    width: '100%',
    height: '100%',
    border: 'none',        // Removes default iframe border
  },
  backButton: {
    position: 'fixed',
    bottom: '20px',
    left: '50%',
    transform: 'translateX(-50%)',
    padding: '10px 25px',
    fontSize: '18px',
    backgroundColor: '#dc3545',
    color: '#fff',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    boxShadow: '0 2px 4px rgba(220, 53, 69, 0.3)',
    zIndex: 2,             // Places button above iframe content
  },
};

export default ARPage;
