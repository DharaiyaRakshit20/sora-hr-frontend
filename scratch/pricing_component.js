var Pr=()=>{
  let[e,t]=(0,C.useState)(15);
  let n=[
    {
      id:`plan_starter`,
      name:`Starter Plan`,
      badge:`FOR SMALL TEAMS`,
      desc:`Essential HR automation & employee directory for growing startups.`,
      basePrice:2499,
      baseMinEmp:1,
      baseMaxEmp:10,
      perEmployeePrice:120,
      popular:!1,
      features:[
        `1-10 Employees Included in Base Slab`,
        `\u20B9120 / additional employee per month`,
        `Employee Directory & Profile Vault`,
        `Geofence & Mobile Attendance (ESS)`,
        `Leave & Holiday Approval Workflows`,
        `Self-Service Mobile App Access`
      ]
    },
    {
      id:`plan_growth`,
      name:`Growth Pro`,
      badge:`MOST POPULAR`,
      desc:`Complete HRMS, statutory payroll calculation, and recruitment suite.`,
      basePrice:5999,
      baseMinEmp:1,
      baseMaxEmp:25,
      perEmployeePrice:150,
      popular:!0,
      features:[
        `1-25 Employees Included in Base Slab`,
        `\u20B9150 / additional employee per month`,
        `Automated Multi-State Statutory Payroll`,
        `Applicant Tracking System (ATS)`,
        `Biometric & Shift Rostering Engine`,
        `EPF, ESIC, PT, TDS Compliance Exports`,
        `Priority 24/7 Dedicated Support`
      ]
    },
    {
      id:`plan_enterprise`,
      name:`Enterprise Elite`,
      badge:`FULL SUITE`,
      desc:`Unlimited power with performance appraisal, training, audit readiness & custom APIs.`,
      basePrice:12999,
      baseMinEmp:1,
      baseMaxEmp:50,
      perEmployeePrice:180,
      popular:!1,
      features:[
        `1-50 Employees Included in Base Slab`,
        `\u20B9180 / additional employee per month`,
        `360\u00B0 KPI & Performance Appraisals`,
        `L&D Training LMS & Certifications`,
        `Statutory Audit & Labor Law Vault`,
        `Dedicated Enterprise Account Manager`,
        `Custom API & ERP Integrations`
      ]
    }
  ];

  let r=planId=>{
    window.location.href=`/checkout/?plan=`+encodeURIComponent(planId)+`&emp=`+e;
  };

  let pct=Math.round(((e-1)/149)*100);
  let sliderBg=`linear-gradient(to right, #FAB005 0%, #E8590C `+pct+`%, rgba(255,255,255,0.18) `+pct+`%, rgba(255,255,255,0.18) 100%)`;

  return(0,R.jsxs)(`section`,{
    id:`pricing`,
    className:`relative w-full py-28 md:py-36 bg-transparent border-t border-white/5`,
    children:[
      (0,R.jsx)(`style`,{
        children:`
          .sora-range {
            -webkit-appearance: none !important;
            appearance: none !important;
            width: 100% !important;
            height: 6px !important;
            border-radius: 9999px !important;
            outline: none !important;
            border: none !important;
            cursor: pointer !important;
            margin: 16px 0 !important;
          }
          .sora-range::-webkit-slider-runnable-track {
            -webkit-appearance: none !important;
            height: 6px !important;
            border-radius: 9999px !important;
            background: transparent !important;
            border: none !important;
          }
          .sora-range::-webkit-slider-thumb {
            -webkit-appearance: none !important;
            appearance: none !important;
            width: 22px !important;
            height: 22px !important;
            border-radius: 50% !important;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%) !important;
            border: 2px solid #FFFFFF !important;
            box-shadow: 0 0 14px rgba(250, 176, 5, 0.95), 0 2px 6px rgba(0,0,0,0.6) !important;
            cursor: pointer !important;
            margin-top: -8px !important;
            transition: transform 0.15s ease, box-shadow 0.15s ease !important;
          }
          .sora-range::-webkit-slider-thumb:hover {
            transform: scale(1.15) !important;
            box-shadow: 0 0 22px rgba(250, 176, 5, 1) !important;
          }
          .sora-range::-moz-range-track {
            height: 6px !important;
            border-radius: 9999px !important;
            background: transparent !important;
            border: none !important;
          }
          .sora-range::-moz-range-thumb {
            width: 22px !important;
            height: 22px !important;
            border-radius: 50% !important;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%) !important;
            border: 2px solid #FFFFFF !important;
            box-shadow: 0 0 14px rgba(250, 176, 5, 0.95) !important;
            cursor: pointer !important;
          }
          .sora-pricing-card {
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            border-radius: 2px;
            background: rgba(10, 10, 14, 0.68) !important;
            -webkit-backdrop-filter: blur(14px) !important;
            backdrop-filter: blur(14px) !important;
            padding: 36px 32px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            transition: all 0.3s ease;
            box-shadow: 0 20px 50px rgba(0, 0, 0, 0.5);
          }
          .sora-pricing-card:hover {
            border-color: rgba(232, 93, 4, 0.6) !important;
            box-shadow: 0 15px 45px rgba(232, 93, 4, 0.22), 0 20px 50px rgba(0, 0, 0, 0.6) !important;
            transform: translateY(-4px);
          }
          .sora-pricing-card-popular {
            border: 1.5px solid #FAB005 !important;
            box-shadow: 0 0 35px rgba(232, 89, 12, 0.3), 0 20px 50px rgba(0, 0, 0, 0.6) !important;
            transform: scale(1.02);
          }
          .sora-headcount-box {
            max-width: 620px;
            margin: 0 auto 56px auto;
            padding: 28px 32px;
            border-radius: 2px;
            background: rgba(10, 10, 14, 0.68) !important;
            -webkit-backdrop-filter: blur(14px) !important;
            backdrop-filter: blur(14px) !important;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 20px 50px rgba(0, 0, 0, 0.5);
          }
          .sora-slab-box {
            padding: 20px;
            border-radius: 2px;
            background: rgba(0, 0, 0, 0.4) !important;
            -webkit-backdrop-filter: blur(8px) !important;
            backdrop-filter: blur(8px) !important;
            border: 1px solid rgba(255, 255, 255, 0.08);
          }
          .sora-glass-text {
            text-shadow: 0 1px 3px rgba(0, 0, 0, 0.9);
          }
          .sora-glass-title {
            text-shadow: 0 2px 6px rgba(0, 0, 0, 0.95);
          }
          .sora-badge-popular {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 4px 12px;
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%);
            color: #050505;
            font-weight: 900;
            font-size: 10px;
            letter-spacing: 0.15em;
            text-transform: uppercase;
            border-radius: 2px;
            margin-bottom: 12px;
            box-shadow: 0 2px 10px rgba(232, 89, 12, 0.4);
          }
          .sora-badge-normal {
            display: inline-flex;
            align-items: center;
            font-weight: 800;
            font-size: 10px;
            letter-spacing: 0.2em;
            text-transform: uppercase;
            color: #FAB005;
            margin-bottom: 12px;
            line-height: 1.6;
            text-shadow: 0 1px 4px rgba(0, 0, 0, 0.9);
          }
          .sora-btn-popular {
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            gap: 8px !important;
            width: 100% !important;
            height: 48px !important;
            padding: 0 24px !important;
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%) !important;
            color: #050505 !important;
            font-weight: 800 !important;
            font-size: 11px !important;
            letter-spacing: 0.12em !important;
            text-transform: uppercase !important;
            border-radius: 2px !important;
            border: none !important;
            cursor: pointer !important;
            box-shadow: 0 0 25px rgba(232, 89, 12, 0.5) !important;
            transition: all 0.3s ease !important;
          }
          .sora-btn-popular:hover {
            box-shadow: 0 0 35px rgba(250, 176, 5, 0.8) !important;
            transform: translateY(-1px) !important;
          }
          .sora-btn-secondary {
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            gap: 8px !important;
            width: 100% !important;
            height: 48px !important;
            padding: 0 24px !important;
            background: rgba(18, 18, 26, 0.75) !important;
            -webkit-backdrop-filter: blur(8px) !important;
            backdrop-filter: blur(8px) !important;
            color: #FFFFFF !important;
            font-weight: 800 !important;
            font-size: 11px !important;
            letter-spacing: 0.12em !important;
            text-transform: uppercase !important;
            border-radius: 2px !important;
            border: 1.5px solid rgba(250, 176, 5, 0.6) !important;
            cursor: pointer !important;
            transition: all 0.3s ease !important;
          }
          .sora-btn-secondary:hover {
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%) !important;
            color: #050505 !important;
            border-color: transparent !important;
            box-shadow: 0 0 25px rgba(250, 176, 5, 0.5) !important;
            transform: translateY(-1px) !important;
          }
        `
      }),
      (0,R.jsxs)(`div`,{
        className:`max-w-7xl mx-auto px-6 sm:px-8 relative z-10`,
        children:[
          // Section Header
          (0,R.jsxs)(`div`,{
            className:`max-w-3xl mb-16`,
            children:[
              (0,R.jsxs)(`div`,{
                className:`inline-flex items-center gap-2 text-xs uppercase tracking-[0.25em] text-[#FBBF24] font-medium mb-4`,
                children:[
                  (0,R.jsx)(`span`,{children:`Flexible SaaS Investment`}),
                  (0,R.jsx)(`span`,{"aria-hidden":`true`,className:`text-[#AAA49A]/50`,children:`\u2022`}),
                  (0,R.jsx)(`span`,{className:`text-[#AAA49A]`,children:`Transparent Pricing`})
                ]
              }),
              (0,R.jsxs)(`h2`,{
                className:`text-3xl sm:text-4xl md:text-5xl font-bold tracking-tight text-[#FFFFFF] leading-[1.15] text-balance mb-6 sora-glass-title`,
                children:[
                  `Transparent Plans. `,
                  (0,R.jsx)(`span`,{className:`text-sora-gradient font-serif italic font-normal`,children:`Scale as You Grow.`})
                ]
              }),
              (0,R.jsx)(`p`,{
                className:`text-base sm:text-lg text-[#AAA49A] font-light leading-relaxed sora-glass-text`,
                children:`Choose your foundational HRMS tier. Adjust your team size below to view real-time dynamic pricing with our transparent per-employee seat model.`
              })
            ]
          }),

          // Headcount Slider Box (Frosted Glass Matching Reference)
          (0,R.jsxs)(`div`,{
            className:`sora-headcount-box`,
            children:[
              (0,R.jsxs)(`div`,{
                className:`flex items-center justify-between gap-4 mb-2`,
                children:[
                  (0,R.jsxs)(`div`,{
                    className:`flex items-center gap-2.5`,
                    children:[
                      (0,R.jsx)(`span`,{className:`w-2 h-2 rounded-full bg-gradient-to-r from-[#E8590C] to-[#FAB005] animate-pulse`}),
                      (0,R.jsx)(`span`,{
                        style:{
                          fontSize:`13px`,
                          fontWeight:`700`,
                          letterSpacing:`0.1em`,
                          textTransform:`uppercase`,
                          color:`#FFFFFF`
                        },
                        className:`sora-glass-text`,
                        children:`Select Your Headcount:`
                      })
                    ]
                  }),
                  (0,R.jsxs)(`div`,{
                    style:{
                      fontFamily:`monospace`,
                      fontWeight:`900`,
                      fontSize:`15px`,
                      color:`#FAB005`,
                      background:`rgba(0, 0, 0, 0.45)`,
                      border:`1px solid rgba(250, 176, 5, 0.5)`,
                      padding:`6px 14px`,
                      borderRadius:`2px`,
                      boxShadow:`0 2px 8px rgba(0, 0, 0, 0.5)`
                    },
                    children:[e,` Employees`]
                  })
                ]
              }),

              // Native Precision Slider (Zero Blue, Perfectly Centered)
              (0,R.jsx)(`input`,{
                type:`range`,
                min:`1`,
                max:`150`,
                value:e,
                onChange:ev=>t(parseInt(ev.target.value)||1),
                className:`sora-range`,
                style:{background:sliderBg}
              }),

              (0,R.jsxs)(`div`,{
                className:`flex justify-between text-[11px] text-[#AAA49A] font-mono pt-1 sora-glass-text`,
                children:[
                  (0,R.jsx)(`span`,{children:`1 Emp (Startup)`}),
                  (0,R.jsx)(`span`,{children:`25 Emp`}),
                  (0,R.jsx)(`span`,{children:`50 Emp`}),
                  (0,R.jsx)(`span`,{children:`100 Emp`}),
                  (0,R.jsx)(`span`,{children:`150+ Emp`})
                ]
              })
            ]
          }),

          // 3 Cards Grid (Frosted Glass with 3D Ribbon Visible, Crystal Clear Text)
          (0,R.jsx)(`div`,{
            className:`grid grid-cols-1 lg:grid-cols-3 gap-8 items-stretch mb-16`,
            children:n.map(item=>{
              let isPop=item.popular;
              let extra=Math.max(0,e-item.baseMaxEmp);
              let extraCost=extra*item.perEmployeePrice;
              let total=item.basePrice+extraCost;

              return(0,R.jsxs)(`div`,{
                key:item.id,
                className:`sora-pricing-card `+(isPop?`sora-pricing-card-popular lg:-translate-y-2 z-10`:``),
                children:[
                  (0,R.jsxs)(`div`,{
                    className:`space-y-6`,
                    children:[
                      (0,R.jsxs)(`div`,{
                        children:[
                          isPop ? (0,R.jsx)(`div`,{
                            className:`sora-badge-popular`,
                            children:item.badge
                          }) : (0,R.jsx)(`div`,{
                            className:`sora-badge-normal`,
                            children:item.badge
                          }),
                          (0,R.jsx)(`h3`,{
                            className:`text-2xl sm:text-3xl font-bold tracking-tight text-[#FFFFFF] sora-glass-title`,
                            children:item.name
                          }),
                          (0,R.jsx)(`p`,{
                            className:`text-xs text-[#EDE8DF] font-light mt-2 leading-relaxed min-h-[36px] sora-glass-text`,
                            children:item.desc
                          })
                        ]
                      }),

                      // Dynamic Price Slab Box (Frosted Glass)
                      (0,R.jsxs)(`div`,{
                        className:`sora-slab-box space-y-2.5`,
                        children:[
                          (0,R.jsxs)(`div`,{
                            className:`flex items-baseline gap-1.5`,
                            children:[
                              (0,R.jsx)(`span`,{
                                className:`text-3xl sm:text-4xl font-bold text-[#FAB005] font-serif sora-glass-title`,
                                children:`\u20B9`+Math.round(total).toLocaleString(`en-IN`)
                              }),
                              (0,R.jsx)(`span`,{
                                className:`text-xs text-neutral-300 font-light sora-glass-text`,
                                children:`/ month`
                              })
                            ]
                          }),
                          (0,R.jsxs)(`div`,{
                            className:`text-[11px] text-neutral-300 flex items-center justify-between font-mono pt-2 border-t border-white/10 sora-glass-text`,
                            children:[
                              (0,R.jsx)(`span`,{children:`Base (`+item.baseMinEmp+`-`+item.baseMaxEmp+` emp):`}),
                              (0,R.jsx)(`span`,{className:`text-white font-medium`,children:`\u20B9`+item.basePrice.toLocaleString(`en-IN`)})
                            ]
                          }),
                          extra>0 ? (0,R.jsxs)(`div`,{
                            className:`text-[11px] text-[#FBBF24] flex items-center justify-between font-mono font-medium sora-glass-text`,
                            children:[
                              (0,R.jsx)(`span`,{children:`+`+extra+` Extra @ \u20B9`+item.perEmployeePrice+`/emp:`}),
                              (0,R.jsx)(`span`,{children:`+\u20B9`+extraCost.toLocaleString(`en-IN`)})
                            ]
                          }) : (0,R.jsx)(`div`,{
                            className:`text-[10px] text-emerald-400 font-medium flex items-center gap-1 sora-glass-text`,
                            children:`\u2713 Included in base slab (Zero extra charges)`
                          })
                        ]
                      }),

                      // Features Checklist
                      (0,R.jsx)(`ul`,{
                        className:`space-y-3 pt-2`,
                        children:item.features.map((feat,idx)=>(0,R.jsxs)(`li`,{
                          key:idx,
                          className:`flex items-start gap-3 text-xs sm:text-[13px] text-neutral-200 font-light leading-snug sora-glass-text`,
                          children:[
                            (0,R.jsx)(`span`,{
                              className:`text-[#FAB005] font-bold text-sm shrink-0 mt-0.5`,
                              children:`\u2713`
                            }),
                            (0,R.jsx)(`span`,{children:feat})
                          ]
                        }))
                      })
                    ]
                  }),

                  // Card Bottom Action Button (Full Height, Luxury Hover)
                  (0,R.jsx)(`div`,{
                    style:{
                      paddingTop:`24px`,
                      marginTop:`24px`,
                      borderTop:`1px solid rgba(255, 255, 255, 0.1)`
                    },
                    children:(0,R.jsxs)(`button`,{
                      type:`button`,
                      onClick:()=>r(item.id),
                      className:isPop ? `sora-btn-popular` : `sora-btn-secondary`,
                      children:[
                        (0,R.jsx)(`span`,{children:`Get Started \u2022 Buy Plan`}),
                        (0,R.jsx)(`span`,{children:`\u2192`})
                      ]
                    })
                  })
                ]
              });
            })
          }),

          // Trust Badges
          (0,R.jsxs)(`div`,{
            className:`pt-8 border-t border-white/10 text-center text-xs text-[#AAA49A] flex flex-wrap items-center justify-center gap-8 sm:gap-12`,
            children:[
              (0,R.jsxs)(`span`,{
                className:`flex items-center gap-2 font-medium text-neutral-300 sora-glass-text`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-emerald-400 font-bold`,children:`\u2713`}),
                  (0,R.jsx)(`span`,{children:`Instant Workspace Activation`})
                ]
              }),
              (0,R.jsxs)(`span`,{
                className:`flex items-center gap-2 font-medium text-neutral-300 sora-glass-text`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-[#FAB005] font-bold`,children:`\u2605`}),
                  (0,R.jsx)(`span`,{children:`Razorpay & UPI Verified`})
                ]
              }),
              (0,R.jsxs)(`span`,{
                className:`flex items-center gap-2 font-medium text-neutral-300 sora-glass-text`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-[#FFE066] font-bold`,children:`\u21BA`}),
                  (0,R.jsx)(`span`,{children:`Cancel or Scale Seats Anytime`})
                ]
              })
            ]
          })
        ]
      })
    ]
  });
};
