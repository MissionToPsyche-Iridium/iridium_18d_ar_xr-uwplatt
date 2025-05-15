/**
 * ParticlesComponent.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: Provides a fullscreen animated particle background using the `react-tsparticles`
 *          library with configurable interactivity and visual effects.
 * Date Written: May 14, 2025
 */

import Particles from "react-tsparticles";
import { loadSlim } from "tsparticles-slim"; // Load optimized slim version for performance
import { useCallback, useMemo } from "react";

/**
 * ParticlesComponent
 *
 * Renders an animated particles effect in the background of the screen.
 * Uses hover and click interactivity to enhance the visual experience.
 */
const ParticlesComponent = () => {
  // Particle options are memoized for performance
  const options = useMemo(() => {
    return {
      background: {
        // Optional background color can be added here
        // color: "#000",
      },
      fullScreen: {
        enable: true, // Enables full-screen rendering behind other content
      },
      interactivity: {
        events: {
          onClick: {
            enable: true,
            mode: "push", // Adds particles on click
          },
          onHover: {
            enable: true,
            mode: "repulse", // Particles repel from cursor
          },
        },
        modes: {
          push: {
            quantity: 10, // Number of particles added per click
          },
          repulse: {
            distance: 100, // Distance from cursor to trigger repulsion
          },
        },
      },
      particles: {
        number: {
          value: 400, // Total number of particles
        },
        move: {
          enable: true,
          speed: { min: 0.2, max: 1.5 }, // Random speed range for natural motion
        },
        links: {
          enable: false, // Disable linking lines between particles
        },
        size: {
          value: { min: 0.5, max: 3 }, // Randomized size for twinkling effect
        },
        // detectRetina: true, // Optional retina optimization
      },
    };
  }, []);

  // Initializes the particles engine with the slim build
  const particlesInit = useCallback((engine) => {
    loadSlim(engine); // Use lightweight preset for performance
    // loadFull(engine); // Optional: Load full version if needed
  }, []);

  return <Particles init={particlesInit} options={options} />;
};

export default ParticlesComponent;
