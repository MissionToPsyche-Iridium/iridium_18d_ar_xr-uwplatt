import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next'; // Import translation hook
import { ReactNebula } from "@flodlc/nebula";
import ARPage from './components/ARPage';
import Credits from './components/Credits';
import LearnMore from './components/LearnMore';
import CustomNavbar from './components/Navbar';
import Videos from './components/Videos';
import Instructions from './components/Instructions';
import './App.css';
import { useGetPerformanceReport } from './loadTime.ts';

const App = () => {
  const { t, i18n } = useTranslation(); // Hook for translations
  const [selectedMode, setSelectedMode] = useState('Home'); // Default to 'Home'

  // changeLanguage function
  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    localStorage.setItem('appLanguage', lng);
  };

  const handleStartAR = () => setSelectedMode('AR');
  const handleBack = () => setSelectedMode('Home');
  const handleHomeClick = () => setSelectedMode('Home');
  const handleCreditClick = () => setSelectedMode('Credits');
  const handleLearnMoreClick = () => setSelectedMode('LearnMore');
  const handleVideosClick = () => setSelectedMode('Videos');
  const handleInstructionsClick = () => selectedMode('Instructions')

  // Handle initial load and hash change to set the correct mode
  useEffect(() => {
    const handleHashChange = () => {
      const hash = window.location.hash.replace('#', '');
      console.log('Hash:', hash);
      if (hash) {
        if (hash === "learnmore") setSelectedMode('LearnMore');
        else if (hash === "videos") setSelectedMode('Videos');
        else if (hash === "credits") setSelectedMode('Credits');
        else if (hash === "instructions") setSelectedMode('Instructions')
      } else {
        setSelectedMode('Home');
      }
    };

    handleHashChange();
    window.addEventListener('hashchange', handleHashChange);
    return () => window.removeEventListener('hashchange', handleHashChange);
  }, []);

  useEffect(() => {
    document.body.classList.add('dark-mode');
  }, []);

  const [feedDataLoaded] = useState(false);
  useGetPerformanceReport({
    screenName: 'App',
    isLoading: feedDataLoaded,
  });

  return (
    <div className={`App ${selectedMode === 'AR' ? 'ar-mode' : ''}`}>
      <CustomNavbar
        onHomeClick={handleHomeClick}
        onCreditClick={handleCreditClick}
        onLearnMoreClick={handleLearnMoreClick}
        onVideoClick={handleVideosClick}
        onInstructionClick={handleInstructionsClick}
        onChangeLanguage={changeLanguage}
      />
      {selectedMode === 'Home' && (
        <div className="start-container">
          <ReactNebula config={{
            starsCount: 700,
            solarSystemOrbite: 50,
            planetsScale: 3,
            sunScale: 1.5,
            cometFrequence: 60,
            solarSystemSpeedOrbit: 220,
            starsRotationSpeed: 15,
          }}/>
          <button className="start-button" onClick={handleStartAR}>
            {t('startAR')}
          </button>
          <button 
            className="instructions-button" 
            onClick={() => setSelectedMode('Instructions')}
          >
            {t('instructions.button', 'Instructions')}
          </button>
        </div>
      )}
      {selectedMode === 'AR' && <ARPage onBack={handleBack} />}
      {selectedMode === 'Credits' && <Credits />}
      {selectedMode === 'LearnMore' && <LearnMore />}
      {selectedMode === 'Videos' && <Videos />}
      {selectedMode === 'Instructions' && <Instructions/>}
    </div>
  );
};

export default App;
