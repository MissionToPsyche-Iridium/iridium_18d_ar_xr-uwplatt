/**
 * App.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: Main entry point for the React application. Handles navigation between
 *          AR experience, Credits, Learn More, Videos, and Instructions views.
 *          Supports localization, background video, and hash-based routing.
 * Date Written: May 14, 2025
 */

import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';

import ARPage from './components/ARPage';
import Credits from './components/Credits';
import LearnMore from './components/LearnMore';
import CustomNavbar from './components/Navbar';
import Videos from './components/Videos';
import psycheVideo from './assets/psyche.mp4';
import Instructions from './components/Instructions';

import './App.css';
import { useGetPerformanceReport } from './loadTime.ts'; // Custom performance logging hook

const App = () => {
  const { t, i18n } = useTranslation(); // Translation hook
  const [selectedMode, setSelectedMode] = useState('Home'); // Tracks which page is displayed

  /**
   * Handles changing the application language
   * and persists it to localStorage.
   */
  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    localStorage.setItem('appLanguage', lng);
  };

  // Navigation event handlers
  const handleStartAR = () => setSelectedMode('AR');
  const handleBack = () => setSelectedMode('Home');
  const handleHomeClick = () => setSelectedMode('Home');
  const handleCreditClick = () => setSelectedMode('Credits');
  const handleLearnMoreClick = () => setSelectedMode('LearnMore');
  const handleVideosClick = () => setSelectedMode('Videos');
  const handleInstructionsClick = () => setSelectedMode('Instructions');

  /**
   * Updates view based on URL hash (e.g. #learnmore)
   * Enables direct linking to internal sections.
   */
  useEffect(() => {
    const handleHashChange = () => {
      const hash = window.location.hash.replace('#', '');
      if (hash === 'learnmore') setSelectedMode('LearnMore');
      else if (hash === 'videos') setSelectedMode('Videos');
      else if (hash === 'credits') setSelectedMode('Credits');
      else if (hash === 'instructions') setSelectedMode('Instructions');
      else setSelectedMode('Home');
    };

    handleHashChange(); // Trigger once on initial load
    window.addEventListener('hashchange', handleHashChange);
    return () => window.removeEventListener('hashchange', handleHashChange);
  }, []);

  // Enable dark mode class on load
  useEffect(() => {
    document.body.classList.add('dark-mode');
  }, []);

  // Simulated loading flag (could be expanded later)
  const [feedDataLoaded] = useState(false);

  // Tracks and reports performance metrics
  useGetPerformanceReport({
    screenName: 'App',
    isLoading: feedDataLoaded,
  });

  return (
    <div className={`App ${selectedMode === 'AR' ? 'ar-mode' : ''}`}>
      {/* Shared navigation across all pages */}
      <CustomNavbar
        onHomeClick={handleHomeClick}
        onCreditClick={handleCreditClick}
        onLearnMoreClick={handleLearnMoreClick}
        onVideoClick={handleVideosClick}
        onInstructionClick={handleInstructionsClick}
        onChangeLanguage={changeLanguage}
      />

      {/* Home screen with looping background video */}
      {selectedMode === 'Home' && (
        <div className="start-container">
          <video autoPlay muted loop playsInline className="background-video">
            <source src={psycheVideo} type="video/mp4" />
            <source src="/psyche.mp4" type="video/webm" />
            {t('videoNotSupported')}
          </video>

          <div className="overlay-content">
            <button className="start-button" onClick={handleStartAR}>
              {t('startAR')}
            </button>

            <button
              className="instructions-button"
              onClick={() => setSelectedMode('Instructions')}
            >
              {t('instructions.button', 'Instructions')}
            </button>

            {/* Language selector dropdown */}
            <select
              value={i18n.language}
              onChange={(e) => changeLanguage(e.target.value)}
              className="language-selector-home"
            >
              <option value="en">English</option>
              <option value="es">Español</option>
              <option value="fr">Français</option>
              <option value="md">中文</option>
            </select>
          </div>
        </div>
      )}

      {/* Conditional rendering for different page modes */}
      {selectedMode === 'AR' && <ARPage onBack={handleBack} />}
      {selectedMode === 'Credits' && <Credits />}
      {selectedMode === 'LearnMore' && <LearnMore />}
      {selectedMode === 'Videos' && <Videos />}
      {selectedMode === 'Instructions' && <Instructions />}
    </div>
  );
};

export default App;
