/**
 * Instructions.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: This component displays a step-by-step instruction guide for users,
 *          using translated text and corresponding instructional images.
 *          It includes visual enhancements such as particle effects and responsive layout.
 * Date Written: May 14, 2025
 */

import React from 'react';
import './Instructions.css'; // Styling for the instructions page
import Particles from './Particles'; // Animated background component
import { useTranslation } from 'react-i18next'; // Translation hook for multilingual support

// Imported instruction images
import image1 from '../assets/InstructionImages/IMG1.PNG';
import image2 from '../assets/InstructionImages/IMG2.PNG';
import image3 from '../assets/InstructionImages/IMG3.PNG';
import image4 from '../assets/InstructionImages/IMG4.PNG';
import image5 from '../assets/InstructionImages/IMG5.PNG';

/**
 * Instructions Component
 *
 * Displays a list of step-by-step instructions, each with a title, description, and image.
 * Uses translations for all textual content.
 */
const Instructions = () => {
  const { t } = useTranslation(); // Access translation strings

  // Steps array contains structured instruction data
  const steps = [
    {
      title: t('instructions.step1.title'),
      text: t('instructions.step1.text'),
      img: image1,
    },
    {
      title: t('instructions.step2.title'),
      text: t('instructions.step2.text'),
      img: image2,
    },
    {
      title: t('instructions.step3.title'),
      text: t('instructions.step3.text'),
      img: image3,
    },
    {
      title: t('instructions.step4.title'),
      text: t('instructions.step4.text'),
      img: image4,
    },
    {
      title: t('instructions.step5.title'),
      text: t('instructions.step5.text'),
      img: image5,
    },
  ];

  return (
    <div className="instructions-container">
      <Particles /> {/* Background animation */}

      {/* Section heading */}
      <h1 className="instructions-title">{t('instructions.title')}</h1>

      {/* Loop through all instruction steps and render them */}
      <div className="instructions-body">
        {steps.map((step, index) => (
          <div key={index} className="instruction-section">
            <h2>{step.title}</h2>
            {step.img && (
              <img
                src={step.img}
                alt={step.title}
                className="instruction-img"
              />
            )}
            <p>{step.text}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Instructions;
