/**
 * LearnMore.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: Displays interactive cards for users to learn more about key AR markers,
 *          each representing an element of the Psyche mission (spacecraft, launch, asteroid, etc.).
 *          Includes translations, toggleable content, and visual effects.
 * Date Written: May 14, 2025
 */

import React, { useState } from 'react';
import './LearnMore.css'; // Styling specific to the Learn More section
import Particles from './Particles'; // Visual background component
import { useTranslation } from 'react-i18next'; // Hook for i18n translations

// Image assets representing different markers
import spacecraft from '../assets/images/Marker1.png';
import asteroid from '../assets/images/Marker2.png';
import launch from '../assets/images/Marker3.png';
import history from '../assets/images/Marker4.png';
import orbit from '../assets/images/Marker5.png';

/**
 * LearnMore Component
 * 
 * Displays a grid of image cards. When a user clicks a card, additional
 * translated text information is revealed underneath the card title.
 * Cards toggle open and closed on click.
 */
const LearnMore = () => {
  const { t } = useTranslation(); // Access translation strings
  const [activeCard, setActiveCard] = useState(null); // Tracks which card is open

  /**
   * Handles user clicking a card:
   * - If already open, it closes it
   * - Otherwise, sets it as the active card
   */
  const handleCardClick = (id) => {
    setActiveCard(activeCard === id ? null : id);
  };

  // Array of card content, each containing an ID, translated title, text, and image
  const content = [
    { id: 1, title: t('learnMore.marker1.title'), text: t('learnMore.marker1.text'), img: spacecraft },
    { id: 2, title: t('learnMore.marker2.title'), text: t('learnMore.marker2.text'), img: asteroid },
    { id: 3, title: t('learnMore.marker3.title'), text: t('learnMore.marker3.text'), img: launch },
    { id: 4, title: t('learnMore.marker4.title'), text: t('learnMore.marker4.text'), img: history },
    { id: 5, title: t('learnMore.marker5.title'), text: t('learnMore.marker5.text'), img: orbit },
  ];

  return (
    <div className="learn-more-container">
      <Particles /> {/* Background particle effect */}

      <h1 className="learn-more-title">Learn More</h1>

      <div className="learn-more-grid">
        {content.map((item) => (
          <div
            key={item.id}
            className={`learn-more-card ${activeCard === item.id ? 'active' : ''}`}
            onClick={() => handleCardClick(item.id)}
          >
            <img src={item.img} alt={item.title} className="learn-more-image" />

            <div className="learn-more-content">
              <h2>{item.title}</h2>
              {activeCard === item.id && (
                <p className="learn-more-text">{item.text}</p> // Only show text for active card
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default LearnMore;
