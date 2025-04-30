import React from 'react';
import './Instructions.css';
import Particles from './Particles';
import { useTranslation } from 'react-i18next';
import image1 from '../assets/InstructionImages/IMG1.PNG';
import image2 from '../assets/InstructionImages/IMG2.PNG';
import image3 from '../assets/InstructionImages/IMG3.PNG';
import image4 from '../assets/InstructionImages/IMG4.PNG';
import image5 from '../assets/InstructionImages/IMG5.PNG';

const Instructions = () => {
  const { t } = useTranslation();

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
      <Particles />
      <h1 className="instructions-title">{t('instructions.title')}</h1>
      <div className="instructions-body">
        {steps.map((step, index) => (
          <div key={index} className="instruction-section">
            <h2>{step.title}</h2>
            {step.img && <img src={step.img} alt={step.title} className="instruction-img" />}
            <p>{step.text}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Instructions;
