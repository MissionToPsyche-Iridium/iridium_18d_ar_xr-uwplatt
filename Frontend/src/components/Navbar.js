/**
 * CustomNavbar.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: A responsive and multilingual navigation bar for the AR web application.
 *          This component includes navigation links for key sections and supports
 *          dynamic click handling via callback props.
 * Date Written: May 14, 2025
 */

import React, { useState } from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';
import { useTranslation } from 'react-i18next'; // Hook for internationalization

/**
 * CustomNavbar Component
 *
 * Props:
 * - onHomeClick: function to handle navigation to Home
 * - onCreditClick: function to handle navigation to Credits
 * - onLearnMoreClick: function to handle navigation to Learn More section
 * - onVideoClick: function to handle navigation to Videos section
 * - onInstructionsClick: function to handle navigation to Instructions
 * - onChangeLanguage: optional function to change language (currently commented out)
 */
const CustomNavbar = ({
  onHomeClick,
  onCreditClick,
  onLearnMoreClick,
  onVideoClick,
  onInstructionsClick,
  onChangeLanguage
}) => {
  const [expanded, setExpanded] = useState(false); // Controls mobile menu expansion
  const { t } = useTranslation(); // Provides access to translation strings

  /**
   * Handles any navigation click by:
   * - invoking the provided callback (if any)
   * - collapsing the navbar
   */
  const handleNavClick = (callback) => {
    if (callback) callback(); // Execute section-specific navigation logic
    setExpanded(false);       // Close navbar after navigation
  };

  return (
    <Navbar expanded={expanded} expand="lg" fixed="top" className="transparent-navbar">
      <Container>
        {/* Brand/logo link - also triggers Home click handler */}
        <Navbar.Brand href="#home" onClick={() => handleNavClick(onHomeClick)}>
          {t('nav.brand')}
        </Navbar.Brand>

        {/* Mobile menu toggle button */}
        <Navbar.Toggle
          aria-controls="basic-navbar-nav"
          onClick={() => setExpanded(!expanded)}
        />

        {/* Collapsible menu items */}
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            {/* Internal navigation links with click callbacks */}
            <Nav.Link href="#home" onClick={() => handleNavClick(onHomeClick)}>
              {t('nav.home')}
            </Nav.Link>
            <Nav.Link href="https://psyche.asu.edu" target="_blank" rel="noopener noreferrer">
              {t('nav.psycheWebsite')}
            </Nav.Link>
            <Nav.Link href="#learnmore" onClick={() => handleNavClick(onLearnMoreClick)}>
              {t('nav.learnMore')}
            </Nav.Link>
            <Nav.Link href="#video" onClick={() => handleNavClick(onVideoClick)}>
              {t('nav.videos')}
            </Nav.Link>
            <Nav.Link href="#credits" onClick={() => handleNavClick(onCreditClick)}>
              {t('nav.credits')}
            </Nav.Link>
            <Nav.Link href="#instructions" onClick={() => handleNavClick(onInstructionsClick)}>
              {t('nav.instructions', 'Instructions')}
            </Nav.Link>
          </Nav>

          {/* Optional Language Selector (currently commented out) */}
          {/*
          <select onChange={(e) => onChangeLanguage(e.target.value)} defaultValue={i18n.language}>
            <option value="en">English</option>
            <option value="es">Español</option>
            <option value="fr">Français</option>
            <option value="md">中文</option>
          </select>
          */}
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default CustomNavbar;
