import Particles from "react-tsparticles"
import {loadSlim} from "tsparticles-slim" //load tsparticles slim
//import {loadFull} from "tsparticles" // load tsparticels
import { useCallback, useMemo } from "react";

const ParticlesComponent = () => {
    const options = useMemo (() => {
        return {
            background: {
                //color: "#000",
            },
            fullScreen: {
                enable: true,
            },
            interactivity: {
                events: {
                    onClick: {
                        enable: true,
                        mode: "push",
                    },
                    onHover: {
                        enable: true,
                        mode: "repulse",
                    },
                },
                modes: {
                    push: {
                        quantity: 10,
                    },
                    repulse: {
                        distance: 100,
                    },
                },
            },
            
            particles: {
                number: {
                    value: 100 // number of particles
                },
                move: {
                    enable: true,
                    speed: {min: 0.2, max: 1.5},
                },
                links: {
                    enable: false, // makes particles float around
                },
                size: {
                    value: {min: 0.5, max: 3}, //randomize particle sizes to give twinkling effect
                }
                //detectRetina: true,
            },
        };
}, []);

    const particlesInit = useCallback((engine) => {
        loadSlim(engine);
        //loadFull(engine);
    },[])
    return <Particles init = {particlesInit} options={options} />
}

export default ParticlesComponent;