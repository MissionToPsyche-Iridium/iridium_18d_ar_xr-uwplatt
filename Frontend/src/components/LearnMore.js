import React, { useState } from 'react';
import './LearnMore.css';
import Particles from './Particles'
import { useTranslation } from 'react-i18next'; // Import translation hook
import spacecraft from '../assets/images/Marker1.png';
import asteroid from '../assets/images/Marker2.png';
import launch from '../assets/images/Marker3.png';
import history from '../assets/images/Marker4.png';
import orbit from '../assets/images/Marker5.png';

const LearnMore = () => {
  const { t } = useTranslation(); // Hook to access translations
  const [activeCard, setActiveCard] = useState(null);

  const handleCardClick = (id) => {
    setActiveCard(activeCard === id ? null : id); // Toggle the clicked card
  };

  const content = [
    { id: 1, title: t('learnMore.marker1.title'), text: t('learnMore.marker1.text'), img: spacecraft },
    { id: 2, title: t('learnMore.marker2.title'), text: t('learnMore.marker2.text'), img: asteroid },
    { id: 3, title: t('learnMore.marker3.title'), text: t('learnMore.marker3.text'), img: launch },
    { id: 4, title: t('learnMore.marker4.title'), text: t('learnMore.marker4.text'), img: history },
    { id: 5, title: t('learnMore.marker5.title'), text: t('learnMore.marker5.text'), img: orbit },
  ];

  return (
    <div className="learn-more-container">
      <Particles />
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
              {activeCard === item.id && <p className="learn-more-text">{item.text}</p>}
            </div>
          </div>
        ))}
      </div>
      
    </div>
  );
};

export default LearnMore;
