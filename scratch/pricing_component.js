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
  let sliderBg=`linear-gradient(to right, #FAB005 0%, #E8590C `+pct+`%, #222230 `+pct+`%, #222230 100%)`;

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
            height: 8px !important;
            border-radius: 9999px !important;
            outline: none !important;
            border: none !important;
            cursor: pointer !important;
            margin: 16px 0 !important;
          }
          .sora-range::-webkit-slider-runnable-track {
            -webkit-appearance: none !important;
            height: 8px !important;
            border-radius: 9999px !important;
            background: transparent !important;
            border: none !important;
          }
          .sora-range::-webkit-slider-thumb {
            -webkit-appearance: none !important;
            appearance: none !important;
            width: 24px !important;
            height: 24px !important;
            border-radius: 50% !important;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%) !important;
            border: 2.5px solid #FFFFFF !important;
            box-shadow: 0 0 16px rgba(250, 176, 5, 0.95), 0 2px 6px rgba(0,0,0,0.6) !important;
            cursor: pointer !important;
            margin-top: -8px !important;
            transition: transform 0.15s ease, box-shadow 0.15s ease !important;
          }
          .sora-range::-webkit-slider-thumb:hover {
            transform: scale(1.15) !important;
            box-shadow: 0 0 24px rgba(250, 176, 5, 1) !important;
          }
          .sora-range::-moz-range-track {
            height: 8px !important;
            border-radius: 9999px !important;
            background: transparent !important;
            border: none !important;
          }
          .sora-range::-moz-range-thumb {
            width: 24px !important;
            height: 24px !important;
            border-radius: 50% !important;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%) !important;
            border: 2.5px solid #FFFFFF !important;
            box-shadow: 0 0 16px rgba(250, 176, 5, 0.95) !important;
            cursor: pointer !important;
          }
          .sora-pricing-card {
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            border-radius: 4px;
            background: #0A0A0E;
            padding: 36px 32px;
            border: 1px solid rgba(255, 255, 255, 0.12);
            transition: all 0.3s ease;
            box-shadow: 0 20px 50px rgba(0, 0, 0, 0.9);
          }
          .sora-pricing-card:hover {
            border-color: rgba(250, 176, 5, 0.5);
            transform: translateY(-2px);
          }
          .sora-pricing-card-popular {
            border: 1.5px solid #FAB005 !important;
            box-shadow: 0 0 45px rgba(232, 89, 12, 0.35), 0 20px 50px rgba(0, 0, 0, 0.95) !important;
            transform: scale(1.02);
          }
          .sora-badge-popular {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 5px 14px;
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%);
            color: #050505;
            font-weight: 900;
            font-size: 10px;
            letter-spacing: 0.15em;
            text-transform: uppercase;
            border-radius: 2px;
            margin-bottom: 14px;
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
            margin-bottom: 14px;
            line-height: 1.6;
          }
          .sora-btn-popular {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            width: 100%;
            padding: 14px 20px !important;
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%);
            color: #050505;
            font-weight: 800;
            font-size: 11px;
            letter-spacing: 0.12em;
            text-transform: uppercase;
            border-radius: 2px;
            border: none;
            cursor: pointer;
            box-shadow: 0 0 25px rgba(232, 89, 12, 0.5);
            transition: all 0.3s ease;
          }
          .sora-btn-popular:hover {
            box-shadow: 0 0 35px rgba(250, 176, 5, 0.8);
            transform: translateY(-1px);
          }
          .sora-btn-secondary {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            width: 100%;
            padding: 14px 20px !important;
            background: #14141F;
            color: #FFFFFF;
            font-weight: 800;
            font-size: 11px;
            letter-spacing: 0.12em;
            text-transform: uppercase;
            border-radius: 2px;
            border: 1.5px solid rgba(250, 176, 5, 0.6);
            cursor: pointer;
            transition: all 0.3s ease;
          }
          .sora-btn-secondary:hover {
            background: linear-gradient(90deg, #E8590C 0%, #FAB005 50%, #FFE066 100%);
            color: #050505;
            border-color: transparent;
            box-shadow: 0 0 25px rgba(250, 176, 5, 0.5);
            transform: translateY(-1px);
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
                  (0,R.jsx)(`span`,{"aria-hidden":`true`,className:`text-[#AAA49A]/50`,children:`•`}),
                  (0,R.jsx)(`span`,{className:`text-[#AAA49A]`,children:`Transparent Pricing`})
                ]
              }),
              (0,R.jsxs)(`h2`,{
                className:`text-3xl sm:text-4xl md:text-5xl font-bold tracking-tight text-[#FFFFFF] leading-[1.15] text-balance mb-6`,
                children:[
                  `Transparent Plans. `,
                  (0,R.jsx)(`span`,{className:`text-sora-gradient font-serif italic font-normal`,children:`Scale as You Grow.`})
                ]
              }),
              (0,R.jsx)(`p`,{
                className:`text-base sm:text-lg text-[#AAA49A] font-light leading-relaxed`,
                children:`Choose your foundational HRMS tier. Adjust your team size below to view real-time dynamic pricing with our transparent per-employee seat model.`
              })
            ]
          }),

          // Headcount Slider Box
          (0,R.jsxs)(`div`,{
            style:{
              maxWidth:`620px`,
              margin:`0 auto 56px auto`,
              padding:`28px 32px`,
              borderRadius:`4px`,
              backgroundColor:`#0B0B10`,
              border:`1px solid rgba(255, 255, 255, 0.12)`,
              boxShadow:`0 20px 50px rgba(0, 0, 0, 0.9)`
            },
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
                        children:`Select Your Headcount:`
                      })
                    ]
                  }),
                  (0,R.jsxs)(`div`,{
                    style:{
                      fontFamily:`monospace`,
                      fontWeight:`900`,
                      fontSize:`16px`,
                      color:`#FAB005`,
                      background:`#151520`,
                      border:`1px solid rgba(250, 176, 5, 0.5)`,
                      padding:`6px 14px`,
                      borderRadius:`3px`,
                      boxShadow:`0 2px 8px rgba(0, 0, 0, 0.5)`
                    },
                    children:[e,` Employees`]
                  })
                ]
              }),

              // Native Range Input Styled with .sora-range (Zero Blue, Perfect Centering)
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
                className:`flex justify-between text-[11px] text-[#AAA49A] font-mono pt-1`,
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

          // 3 Cards Grid (Solid Obsidian Black, Zero Ribbon Interference, Crisp Buttons)
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
                            className:`text-2xl sm:text-3xl font-bold tracking-tight text-[#FFFFFF]`,
                            children:item.name
                          }),
                          (0,R.jsx)(`p`,{
                            className:`text-xs text-[#AAA49A] font-light mt-2 leading-relaxed min-h-[36px]`,
                            children:item.desc
                          })
                        ]
                      }),

                      // Dynamic Price Calculation Slab Box (Solid Crisp Black)
                      (0,R.jsxs)(`div`,{
                        style:{
                          padding:`20px`,
                          borderRadius:`3px`,
                          backgroundColor:`#050507`,
                          border:`1px solid rgba(255, 255, 255, 0.08)`
                        },
                        className:`space-y-2.5`,
                        children:[
                          (0,R.jsxs)(`div`,{
                            className:`flex items-baseline gap-1.5`,
                            children:[
                              (0,R.jsx)(`span`,{
                                className:`text-3xl sm:text-4xl font-bold text-[#FAB005] font-serif`,
                                children:`\u20B9`+Math.round(total).toLocaleString(`en-IN`)
                              }),
                              (0,R.jsx)(`span`,{
                                className:`text-xs text-[#AAA49A] font-light`,
                                children:`/ month`
                              })
                            ]
                          }),
                          (0,R.jsxs)(`div`,{
                            className:`text-[11px] text-[#AAA49A] flex items-center justify-between font-mono pt-2 border-t border-white/5`,
                            children:[
                              (0,R.jsx)(`span`,{children:`Base (`+item.baseMinEmp+`-`+item.baseMaxEmp+` emp):`}),
                              (0,R.jsx)(`span`,{className:`text-white font-medium`,children:`\u20B9`+item.basePrice.toLocaleString(`en-IN`)})
                            ]
                          }),
                          extra>0 ? (0,R.jsxs)(`div`,{
                            className:`text-[11px] text-[#FBBF24] flex items-center justify-between font-mono font-medium`,
                            children:[
                              (0,R.jsx)(`span`,{children:`+`+extra+` Extra @ \u20B9`+item.perEmployeePrice+`/emp:`}),
                              (0,R.jsx)(`span`,{children:`+\u20B9`+extraCost.toLocaleString(`en-IN`)})
                            ]
                          }) : (0,R.jsx)(`div`,{
                            className:`text-[10px] text-emerald-400 font-medium flex items-center gap-1`,
                            children:`✓ Included in base slab (Zero extra charges)`
                          })
                        ]
                      }),

                      // Features Checklist
                      (0,R.jsx)(`ul`,{
                        className:`space-y-3 pt-2`,
                        children:item.features.map((feat,idx)=>(0,R.jsxs)(`li`,{
                          key:idx,
                          className:`flex items-start gap-3 text-xs sm:text-[13px] text-neutral-200 font-light leading-snug`,
                          children:[
                            (0,R.jsx)(`span`,{
                              className:`text-[#FAB005] font-bold text-sm shrink-0 mt-0.5`,
                              children:`✓`
                            }),
                            (0,R.jsx)(`span`,{children:feat})
                          ]
                        }))
                      })
                    ]
                  }),

                  // Card Bottom Action Button (Thick, Prominent, High-Visibility)
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
                        (0,R.jsx)(`span`,{children:`Get Started • Buy Plan`}),
                        (0,R.jsx)(`span`,{children:`→`})
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
                className:`flex items-center gap-2 font-medium text-neutral-300`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-emerald-400 font-bold`,children:`✓`}),
                  (0,R.jsx)(`span`,{children:`Instant Workspace Activation`})
                ]
              }),
              (0,R.jsxs)(`span`,{
                className:`flex items-center gap-2 font-medium text-neutral-300`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-[#FAB005] font-bold`,children:`★`}),
                  (0,R.jsx)(`span`,{children:`Razorpay & UPI Verified`})
                ]
              }),
              (0,R.jsxs)(`span`,{
                className:`flex items-center gap-2 font-medium text-neutral-300`,
                children:[
                  (0,R.jsx)(`span`,{className:`text-[#FFE066] font-bold`,children:`↺`}),
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
