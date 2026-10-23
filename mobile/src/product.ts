export type JourneyStep = { title: string; detail: string; done: boolean };

export type ProductConfig = {
  name: string;
  eyebrow: string;
  title: string;
  subtitle: string;
  accent: string;
  accentSoft: string;
  metric: string;
  metricLabel: string;
  steps: JourneyStep[];
  cardTitle: string;
  cardDetail: string;
  badge: string;
  action: string;
  tabs: string[];
};

export const product: ProductConfig = {
  name: 'RaceHub',
  eyebrow: 'JORNADA 10 KM',
  title: 'Sua próxima corrida já tem um caminho.',
  subtitle: 'Golden Run São Paulo · 10 km',
  accent: '#E6533D',
  accentSoft: '#F9DED8',
  metric: '18 dias',
  metricLabel: 'até a largada',
  steps: [
    { title: 'Inscrição confirmada', detail: 'Hoje, 09:42', done: true },
    { title: 'Kit em preparação', detail: 'Previsão para 28 de outubro', done: true },
    { title: 'Retirada do kit', detail: 'Shopping Cidade Jardim', done: false },
  ],
  cardTitle: 'Entrega do kit',
  cardDetail: 'Endereço validado',
  badge: 'AÇÃO',
  action: 'Acompanhar entrega',
  tabs: ['Início', 'Corridas', 'Favoritos', 'Perfil'],
};
