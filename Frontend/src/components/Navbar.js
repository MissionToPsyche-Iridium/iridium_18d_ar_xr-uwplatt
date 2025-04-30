import React, { useState } from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';
import { useTranslation } from 'react-i18next'; // Import translation hook

const CustomNavbar = ({ onHomeClick, onCreditClick, onLearnMoreClick, onVideoClick, onInstructionsClick, onChangeLanguage }) => {
  const [expanded, setExpanded] = useState(false);
  const { t, i18n } = useTranslation(); // Hook to access translations

  const handleNavClick = (callback) => {
    if (callback) callback(); // Call the provided callback function
    setExpanded(false); // Collapse the navbar
  };

  return (
    <Navbar expanded={expanded} expand="lg" fixed="top" className="transparent-navbar">
      <Container>
        <Navbar.Brand href="#home" onClick={() => handleNavClick(onHomeClick)}>
          {t('nav.brand')}
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" onClick={() => setExpanded(!expanded)} />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
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
          {/* Language Selector Inside Navbar */}
          <select onChange={(e) => onChangeLanguage(e.target.value)} defaultValue={i18n.language}>
            <option value="en">English</option>
            <option value="es">Español</option>
            <option value="fr">Français</option>
            <option value="md">中文</option>
          </select>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default CustomNavbar;