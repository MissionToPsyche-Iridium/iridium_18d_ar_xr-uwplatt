/**
 * ARVRToggle.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: A toggle component that allows the user to switch between AR and VR modes.
 *          Includes styling for light/dark themes and passes the selected mode back to the parent on confirmation.
 * Date Written: May 14, 2025
 */

import React, { useState } from 'react';

/**
 * ARVRToggle Component
 *
 * Props:
 * - onConfirm: function that receives the selected mode ('AR' or 'VR') when the user clicks confirm
 * - darkMode: boolean to toggle between dark/light UI themes
 */
const ARVRToggle = ({ onConfirm, darkMode }) => {
  const [mode, setMode] = useState('AR'); // Default mode is AR

  // Toggles between AR and VR modes
  const handleToggle = () => {
    setMode((prevMode) => (prevMode === 'AR' ? 'VR' : 'AR'));
  };

  // Sends the current mode to the parent component
  const handleConfirm = () => {
    onConfirm(mode);
  };

  return (
    <div style={{ ...styles.container, backgroundColor: darkMode ? '#444' : '#f9f9f9' }}>
      <h2 style={{ ...styles.heading, color: darkMode ? '#f0f0f0' : '#333' }}>
        Choose Your Experience
      </h2>

      {/* Toggle button to switch modes */}
      <button
        style={{
          ...styles.toggleButton,
          backgroundColor: '#007bff', // Consistent blue color for toggle
        }}
        onClick={handleToggle}
      >
        {mode === 'AR' ? 'Switch to VR' : 'Switch to AR'}
      </button>

      {/* Confirmation button */}
      <div style={{ marginTop: '30px' }}>
        <button style={styles.confirmButton} onClick={handleConfirm}>
          Confirm
        </button>
      </div>
    </div>
  );
};

/**
 * Inline styles for the component
 */
const styles = {
  container: {
    textAlign: 'center',
    marginTop: '50px',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    maxWidth: '400px',
    margin: '50px auto',
    transition: 'background-color 0.3s',
  },
  heading: {
    fontSize: '24px',
    transition: 'color 0.3s',
  },
  toggleButton: {
    padding: '10px 20px',
    fontSize: '16px',
    color: '#fff',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    boxShadow: '0 2px 4px rgba(0, 123, 255, 0.3)',
    transition: 'background-color 0.3s, transform 0.3s',
  },
  confirmButton: {
    padding: '10px 25px',
    fontSize: '18px',
    color: '#fff',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    backgroundColor: '#28a745',
    boxShadow: '0 2px 4px rgba(40, 167, 69, 0.3)',
    transition: 'background-color 0.3s, transform 0.3s',
  },
};

export default ARVRToggle;
