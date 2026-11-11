var Pr=()=>{
  let[e,t]=(0,C.useState)(15);
  let n=[
    {
      id:`plan_starter`,
      name:`Starter Plan`,
      badge:`For Small Teams`,
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
      badge:`Most Popular`,
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
      badge:`Full Suite`,
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

  let r=t=>{
    window.location.href=`/checkout/?plan=`+encodeURIComponent(t)+`&emp=`+e;
  };

  let pct=Math.round(((e-1)/149)*100);
  let sliderBg=`linear-gradient(to right, #FAB005 0%, #E8590C `+pct+`%, #1e1e28 `+pct+`%, #1e1e28 100%)`;

  return(0,R.jsxs)(`section`,{
    id:`pricing`,
    className:`relative w-full py-28 md:py-36 bg-[#080808] border-t border-white/10 overflow-hidden`,
    children:[
      (0,R.jsx)(`style`,{
        children:`
          .sora-slider {
            -webkit-appearance: none !important;
            appearance: none !important;
            outline: none !important;
            border: none !important;
          }
          .sora-slider::-webkit-slider-thumb {
            -webkit-appearance: none !important;
            appearance: none !important;
            width: 22px;
            height: 22px;
            border-radius: 50%;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%);
            border: 2px solid #FFFFFF;
            box-shadow: 0 0 14px rgba(250, 176, 5, 0.9), 0 2px 4px rgba(0,0,0,0.6);
            cursor: pointer;
            transition: transform 0.15s ease;
          }
          .sora-slider::-webkit-slider-thumb:hover {
            transform: scale(1.15);
          }
          .sora-slider::-moz-range-thumb {
            width: 22px;
            height: 22px;
            border-radius: 50%;
            background: linear-gradient(135deg, #FFE066 0%, #FAB005 50%, #E8590C 100%);
            border: 2px solid #FFFFFF;
            box-shadow: 0 0 14px rgba(250, 176, 5, 0.9);
            cursor: pointer;
          }
          .sora-slider::-moz-range-track {
            background: transparent;
            border: none;
          }
        `
      }),
      (0,R.jsxs)(`div`,{
        className:`max-w-7xl mx-auto px-6 sm:px-8 relative z-10`,
        children:[
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

          // Headcount Slider Box (Solid, High-Contrast, Zero Blue)
          (0,R.jsxs)(`div`,{
            className:`max-w-2xl mx-auto p-6 sm:p-8 rounded-2xl bg-[#0e0e16]/95 border border-[#E8590C]/40 shadow-[0_20px_50px_rgba(0,0,0,0.8)] backdrop-blur-2xl mb-16 space-y-6 transition-all duration-300`,
            children:[
              (0,R.jsxs)(`div`,{
                className:`flex items-center justify-between gap-4`,
                children:[
                  (0,R.jsxs)(`div`,{
                    className:`flex items-center gap-2.5`,
                    children:[
                      (0,R.jsx)(`span`,{className:`w-2.5 h-2.5 rounded-full bg-gradient-to-r from-[#E8590C] to-[#FAB005] animate-pulse`}),
                      (0,R.jsx)(`span`,{
                        className:`text-xs sm:text-sm font-bold tracking-wider uppercase text-[#FFFFFF]`,
                        children:`Select Your Headcount:`
                      })
                    ]
                  }),
                  (0,R.jsxs)(`div`,{
                    className:`font-mono font-black text-base sm:text-lg text-[#FAB005] bg-[#14141f] border border-[#FAB005]/70 px-4 py-1.5 rounded-md shadow-md`,
                    children:[e,` Employees`]
                  })
                ]
              }),
              (0,R.jsx)(`input`,{
                type:`range`,
                min:`1`,
                max:`150`,
                value:e,
                onChange:e=>t(parseInt(e.target.value)||1),
                style:{background:sliderBg},
                className:`sora-slider w-full cursor-pointer h-2.5 rounded-full border border-white/10`
              }),
              (0,R.jsxs)(`div`,{
                className:`flex justify-between text-xs text-neutral-400 font-mono pt-1`,
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

          // 3 Cards Grid (Solid Dark Backgrounds, High-Contrast Text, No Badge Collision)
          (0,R.jsx)(`div`,{
            className:`grid grid-cols-1 lg:grid-cols-3 gap-8 items-stretch mb-16`,
            children:n.map(t=>{
              let a=t.popular,
                  o=Math.max(0,e-t.baseMaxEmp),
                  i=o*t.perEmployeePrice,
                  s=t.basePrice+i;
              return(0,R.jsxs)(`div`,{
                key:t.id,
                className:`relative flex flex-col justify-between rounded-2xl bg-[#0c0c14]/95 backdrop-blur-2xl transition-all duration-300 border ${
                  a
                    ? `border-[#FAB005] shadow-[0_20px_60px_rgba(232,89,12,0.35)] ring-1 ring-[#FAB005]/50 scale-[1.02] lg:-translate-y-2 z-10 pt-12 pb-8 px-8 sm:px-9`
                    : `border-white/15 hover:border-[#FAB005]/50 hover:scale-[1.01] shadow-[0_15px_40px_rgba(0,0,0,0.7)] p-8 sm:p-9`
                }`,
                children:[
                  // Clean Centered Badge for Popular with plenty of spacing
                  a ? (0,R.jsx)(`div`,{
                    className:`absolute top-0 left-1/2 -translate-x-1/2 -translate-y-1/2 px-5 py-1.5 rounded-full bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-black text-[11px] uppercase tracking-widest shadow-xl whitespace-nowrap`,
                    children:t.badge
                  }) : null,

                  (0,R.jsxs)(`div`,{
                    className:`space-y-6`,
                    children:[
                      (0,R.jsxs)(`div`,{
                        children:[
                          a ? null : (0,R.jsx)(`span`,{
                            className:`text-[10px] uppercase tracking-[0.25em] font-bold text-[#FAB005] block mb-2`,
                            children:t.badge
                          }),
                          (0,R.jsx)(`h3`,{
                            className:`text-2xl sm:text-3xl font-bold tracking-tight text-[#FFFFFF]`,
                            children:t.name
                          }),
                          (0,R.jsx)(`p`,{
                            className:`text-xs text-neutral-300 font-light mt-2 leading-relaxed`,
                            children:t.desc
                          })
                        ]
                      }),

                      // Dynamic Price Calculation Slab Box (High Contrast)
                      (0,R.jsxs)(`div`,{
                        className:`p-5 rounded-xl bg-[#050508] border border-white/10 space-y-3`,
                        children:[
                          (0,R.jsxs)(`div`,{
                            className:`flex items-baseline gap-1.5`,
                            children:[
                              (0,R.jsx)(`span`,{
                                className:`text-3xl sm:text-4xl font-black text-[#FAB005] font-serif`,
                                children:`\u20B9`+Math.round(s).toLocaleString(`en-IN`)
                              }),
                              (0,R.jsx)(`span`,{
                                className:`text-xs text-neutral-400 font-medium`,
                                children:`/ month`
                              })
                            ]
                          }),
                          (0,R.jsxs)(`div`,{
                            className:`text-xs text-neutral-300 flex items-center justify-between font-mono pt-2.5 border-t border-white/10`,
                            children:[
                              (0,R.jsx)(`span`,{children:`Base (`+t.baseMinEmp+`-`+t.baseMaxEmp+` emp):`}),
                              (0,R.jsx)(`span`,{className:`font-semibold text-white`,children:`\u20B9`+t.basePrice.toLocaleString(`en-IN`)})
                            ]
                          }),
                          o > 0 ? (0,R.jsxs)(`div`,{
                            className:`text-xs text-[#FBBF24] flex items-center justify-between font-mono font-semibold`,
                            children:[
                              (0,R.jsx)(`span`,{children:`+`+o+` Extra @ \u20B9`+t.perEmployeePrice+`/emp:`}),
                              (0,R.jsx)(`span`,{children:`+\u20B9`+i.toLocaleString(`en-IN`)})
                            ]
                          }) : (0,R.jsx)(`div`,{
                            className:`text-[11px] text-emerald-400 font-medium flex items-center gap-1`,
                            children:`✓ Included in base slab (Zero extra charges)`
                          })
                        ]
                      }),

                      // Features Checklist (Solid Readable Neutral-200 Text)
                      (0,R.jsx)(`ul`,{
                        className:`space-y-3 pt-2`,
                        children:t.features.map((e,t)=>(0,R.jsxs)(`li`,{
                          key:t,
                          className:`flex items-start gap-3 text-xs sm:text-[13px] text-neutral-200 font-normal leading-snug`,
                          children:[
                            (0,R.jsx)(`span`,{
                              className:`text-[#FAB005] font-bold text-sm shrink-0 mt-0.5`,
                              children:`✓`
                            }),
                            (0,R.jsx)(`span`,{children:e})
                          ]
                        }))
                      })
                    ]
                  }),

                  // CTA Button
                  (0,R.jsx)(`div`,{
                    className:`pt-8 mt-6 border-t border-white/10`,
                    children:(0,R.jsxs)(`button`,{
                      type:`button`,
                      onClick:()=>r(t.id),
                      className:a
                        ? `w-full py-4 px-6 rounded-sm bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-black text-xs uppercase tracking-widest hover:shadow-[0_0_28px_rgba(232,89,12,0.6)] transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`
                        : `w-full py-4 px-6 rounded-sm border border-[#E8590C]/50 bg-[#12121c] text-[#FFFFFF] hover:border-[#FAB005] hover:text-[#FAB005] hover:bg-[#E8590C]/20 font-bold text-xs uppercase tracking-widest transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`,
                      children:[
                        (0,R.jsx)(`span`,{children:`Get Started • Buy Plan`}),
                        (0,R.jsx)(`span`,{children:`→`})
                      ]
                    })
                  })
                ]
              })
            })
          }),

          // Trust Verification Badges
          (0,R.jsxs)(`div`,{
            className:`pt-8 border-t border-white/10 text-center text-xs text-neutral-400 flex flex-wrap items-center justify-center gap-8 sm:gap-12`,
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
