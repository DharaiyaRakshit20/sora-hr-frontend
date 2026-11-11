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
        `₹120 / additional employee per month`,
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
        `₹150 / additional employee per month`,
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
        `₹180 / additional employee per month`,
        `360° KPI & Performance Appraisals`,
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

  return(0,R.jsx)(`section`,{
    id:`pricing`,
    className:`relative w-full py-28 md:py-36 bg-transparent border-t border-white/5`,
    children:(0,R.jsxs)(`div`,{
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

        (0,R.jsxs)(`div`,{
          className:`max-w-2xl mx-auto p-6 sm:p-8 rounded-xl bg-[#080808]/80 border border-[#E8590C]/30 hover:border-[#FAB005]/60 shadow-[0_0_35px_rgba(232,89,12,0.15)] backdrop-blur-md mb-16 space-y-5 transition-all duration-300`,
          children:[
            (0,R.jsxs)(`div`,{
              className:`flex items-center justify-between gap-4`,
              children:[
                (0,R.jsxs)(`div`,{
                  className:`flex items-center gap-2.5`,
                  children:[
                    (0,R.jsx)(`span`,{className:`w-2.5 h-2.5 rounded-full bg-gradient-to-r from-[#E8590C] to-[#FAB005] animate-pulse`}),
                    (0,R.jsx)(`span`,{className:`text-xs sm:text-sm font-semibold tracking-wider uppercase text-[#FFFFFF]`,children:`Select Your Headcount:`})
                  ]
                }),
                (0,R.jsxs)(`div`,{
                  className:`font-mono font-bold text-base sm:text-lg text-[#FAB005] bg-gradient-to-r from-[#E8590C]/20 to-[#FAB005]/20 border border-[#FAB005]/40 px-4 py-1.5 rounded-sm shadow-sm`,
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
              className:`w-full cursor-pointer h-2 bg-white/10 rounded-lg appearance-none accent-[#FAB005]`
            }),
            (0,R.jsxs)(`div`,{
              className:`flex justify-between text-[11px] text-[#AAA49A] font-mono`,
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

        (0,R.jsx)(`div`,{
          className:`grid grid-cols-1 lg:grid-cols-3 gap-8 items-stretch mb-16`,
          children:n.map(t=>{
            let a=t.popular,
                o=Math.max(0,e-t.baseMaxEmp),
                i=o*t.perEmployeePrice,
                s=t.basePrice+i;
            return(0,R.jsxs)(`div`,{
              key:t.id,
              className:`relative flex flex-col justify-between p-8 sm:p-9 rounded-xl bg-[#080808]/75 backdrop-blur-md transition-all duration-300 border ${a?`border-[#FAB005] shadow-[0_0_40px_rgba(232,89,12,0.35)] ring-1 ring-[#FAB005]/40 scale-[1.02] lg:-translate-y-2 z-10`:`border-white/10 hover:border-[#FAB005]/40 hover:scale-[1.01]`}`,
              children:[
                a?(0,R.jsx)(`div`,{
                  className:`absolute -top-3.5 left-1/2 -translate-x-1/2 px-4 py-1 rounded-sm bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-bold text-[10px] uppercase tracking-widest shadow-lg whitespace-nowrap`,
                  children:t.badge
                }):null,
                (0,R.jsxs)(`div`,{
                  className:`space-y-6`,
                  children:[
                    (0,R.jsxs)(`div`,{
                      children:[
                        a?null:(0,R.jsx)(`span`,{className:`text-[10px] uppercase tracking-[0.25em] font-semibold text-[#FAB005] block mb-2`,children:t.badge}),
                        (0,R.jsx)(`h3`,{className:`text-2xl font-bold tracking-tight text-[#FFFFFF]`,children:t.name}),
                        (0,R.jsx)(`p`,{className:`text-xs text-[#AAA49A] font-light mt-2 leading-relaxed`,children:t.desc})
                      ]
                    }),
                    (0,R.jsxs)(`div`,{
                      className:`p-5 rounded-lg bg-[#050505]/90 border border-white/5 space-y-2.5`,
                      children:[
                        (0,R.jsxs)(`div`,{
                          className:`flex items-baseline gap-1.5`,
                          children:[
                            (0,R.jsx)(`span`,{className:`text-3xl sm:text-4xl font-bold text-[#FAB005] font-serif`,children:`₹`+Math.round(s).toLocaleString(`en-IN`)}),
                            (0,R.jsx)(`span`,{className:`text-xs text-[#AAA49A] font-light`,children:`/ month`})
                          ]
                        }),
                        (0,R.jsxs)(`div`,{
                          className:`text-[11px] text-[#AAA49A] flex items-center justify-between font-mono pt-2 border-t border-white/5`,
                          children:[
                            (0,R.jsx)(`span`,{children:`Base (`+t.baseMinEmp+`-`+t.baseMaxEmp+` emp):`}),
                            (0,R.jsx)(`span`,{children:`₹`+t.basePrice.toLocaleString(`en-IN`)})
                          ]
                        }),
                        o>0?(0,R.jsxs)(`div`,{
                          className:`text-[11px] text-[#FBBF24] flex items-center justify-between font-mono`,
                          children:[
                            (0,R.jsx)(`span`,{children:`+`+o+` Extra @ ₹`+t.perEmployeePrice+`/emp:`}),
                            (0,R.jsx)(`span`,{children:`+₹`+i.toLocaleString(`en-IN`)})
                          ]
                        }):(0,R.jsx)(`div`,{
                          className:`text-[10px] text-emerald-400 font-medium flex items-center gap-1`,
                          children:`✓ Included in base slab (Zero extra charges)`
                        })
                      ]
                    }),
                    (0,R.jsx)(`ul`,{
                      className:`space-y-3 pt-2`,
                      children:t.features.map((e,t)=>(0,R.jsxs)(`li`,{
                        key:t,
                        className:`flex items-start gap-3 text-xs sm:text-[13px] text-[#AAA49A] font-light leading-snug`,
                        children:[
                          (0,R.jsx)(`span`,{className:`text-[#FAB005] shrink-0 mt-0.5`,children:`✓`}),
                          (0,R.jsx)(`span`,{children:e})
                        ]
                      }))
                    })
                  ]
                }),
                (0,R.jsx)(`div`,{
                  className:`pt-8 mt-6 border-t border-white/10`,
                  children:(0,R.jsxs)(`button`,{
                    type:`button`,
                    onClick:()=>r(t.id),
                    className:a?`w-full py-3.5 px-6 rounded-sm bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-semibold text-xs uppercase tracking-widest hover:shadow-[0_0_24px_rgba(232,89,12,0.5)] transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`:`w-full py-3.5 px-6 rounded-sm border border-[#E8590C]/40 bg-[#080808]/70 text-[#FFFFFF] hover:border-[#FAB005] hover:text-[#FAB005] hover:bg-[#E8590C]/10 font-semibold text-xs uppercase tracking-widest transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`,
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

        (0,R.jsxs)(`div`,{
          className:`pt-8 border-t border-white/10 text-center text-xs text-[#AAA49A] flex flex-wrap items-center justify-center gap-8 sm:gap-12`,
          children:[
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-emerald-400`,children:`✓`}),
                (0,R.jsx)(`span`,{children:`Instant Workspace Activation`})
              ]
            }),
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-[#FAB005]`,children:`★`}),
                (0,R.jsx)(`span`,{children:`Razorpay & UPI Verified`})
              ]
            }),
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-[#FFE066]`,children:`↺`}),
                (0,R.jsx)(`span`,{children:`Cancel or Scale Seats Anytime`})
              ]
            })
          ]
        })
      ]
    })
  });
};
